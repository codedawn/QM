using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace QM
{
    public class ServerComp : Component
    {
        private List<IFilter> chainFilters;
        private ILog _log;
        private MessageDispatcher _messageDispatcher;
        private Application _application;
        private ServerDispatcher _serverDispatcher;
        private long _responseCount;

        public ServerComp(Application application)
        {
            _application = application;
            chainFilters = new List<IFilter>();
            _log = new NLogger(typeof(ServerComp));
            InitFliter();
        }

        private void InitFliter()
        {
            foreach (Type type in CodeType.Instance.GetTypes(typeof(FilterAttribute)))
            {
                FilterAttribute filterAttribute = type.GetCustomAttribute<FilterAttribute>();
                if (filterAttribute != null)
                {
                    List<string> includes = new List<string>(filterAttribute.includeServer.Split(','));
                    List<string> excludes = new List<string>(filterAttribute.excludeServer.Split(','));

                    string serverType = _application.serverType;
                    if (excludes.IndexOf(FilterAttribute.All) == -1 && excludes.IndexOf(serverType) == -1 && includes.IndexOf(serverType) != -1)
                    {
                        IFilter filter = (IFilter)Activator.CreateInstance(type);
                        chainFilters.Add(filter);
                    }
                }
            }
        }

        public override void Start()
        {
            _serverDispatcher = new ServerDispatcher(_application);
            _messageDispatcher = new MessageDispatcher(_application);
            base.Start();
        }

        public override void AfterStart()
        {
            base.AfterStart();
        }


        public async Task GlobalHandleAsync(IMessage message, ISession session)
        {
            if (state != ComponentState.AfterStart)
            {
                _log.Error("Server compoent状态不是AfterStart,不能处理信息");
                return;
            }

            RouteInfo routeInfo = ParseRoute(message);

            if (!routeInfo.IsValid())
            {
                _log.Error($"收到来自客户端的非法消息message:{message.ToString()}，直接过滤");
                return;
            }

            IResponse response = null;
            bool isFilter = false;
            bool isCrashError = false;
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                foreach (IFilter filter in chainFilters)
                {
                    if (!await filter.Before(message, session))
                    {
                        isFilter = true;
                        break;
                    }
                }
                stopwatch.Stop();
                _log.Debug($"执行beforeFilter耗时：${stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception e)
            {
                isCrashError = true;
                if (message is IRequest request)
                {
                    response = new ErrorResponse() { Id = request.Id, Code = (int)NetworkCode.InternalError, Message = e.ToString() };
                }
                _log.Error(e);
            }

            //before可以拦截消息，不会继续传播
            if (isFilter)
            {
                return;
            }
            if (isCrashError)
            {
                await SendReponseAsync(response, session);
                return;
            }

            try
            {
                response = await _messageDispatcher.DispatchAsync(message, session, routeInfo);
            }
            catch (Exception e)
            {
                isCrashError = true;
                if (message is IRequest request)
                {
                    response = new ErrorResponse() { Id = request.Id, Code = (int)NetworkCode.InternalError, Message = e.ToString() };
                }
                _log.Error(e);
            }
            await SendReponseAsync(response, session);

            foreach (IFilter filter in chainFilters)
            {
                await filter.After(message, response, session);
            }
        }

        private RouteInfo ParseRoute(IMessage message)
        {
            bool isRequest = false;
            bool isNotify = false;
            if (message is IRequest)
            {
                isRequest = true;
            }
            else if (message is INotify)
            {
                isNotify = true;
            }

            RouteInfo routeInfo = new RouteInfo() { isNotify = isNotify, isRequest = isRequest };
            routeInfo.ServerType = _serverDispatcher.Dispatch(message);
            return routeInfo;
        }

        public async Task SendReponseAsync(IResponse response, ISession session)
        {
            if (response != null)
            {
                _responseCount++;
                _log.Debug($"发送response总数：{_responseCount}");
                try
                {
                    await session.Send(response);
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
