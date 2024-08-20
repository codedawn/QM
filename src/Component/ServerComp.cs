using System.Reflection;

namespace QM
{
    public class ServerComp : Component
    {
        private List<IFilter> chainFilters;
        private ILog log;
        private MessageDispatcher _messageDispatcher;
        private Application _application;
        private ServerDispatcher _serverDispatcher;

        public ServerComp(Application application)
        {
            _application = application;
            chainFilters = new List<IFilter>();
            log = new ConsoleLog();
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


        public void GlobalHandle(IMessage message, ISession session)
        {
            if (state != ComponentState.AfterStart)
            {
                log.Error("Server compoent状态不是AfterStart,不能处理信息");
                return;
            }

            RouteInfo routeInfo = ParseRoute(message);

            if (!routeInfo.IsValid())
            {
                log.Error($"收到来自客户端的非法消息message:{message.ToString()}，直接过滤");
                return;
            }

            IResponse response = null;
            bool isFilter = false;
            bool isCrashError = false;
            try
            {
                foreach (IFilter filter in chainFilters)
                {
                    if (!filter.Before(message, session))
                    {
                        isFilter = true;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                isCrashError = true;
                if (message is IRequest request)
                {
                    response = new ErrorResponse() { Id = request.Id, Code = (int)NetworkCode.InternalError, Message = e.Message };
                }
                log.Error(e.ToString());
#if DEBUG
                throw;
#endif
            }

            //before可以拦截消息，不会继续传播
            if (isFilter)
            {
                return;
            }
            if (isCrashError)
            {
                SendReponse(response, session);
                return;
            }

            try
            {
                response = _messageDispatcher.Dispatch(message, session, routeInfo);
            }
            catch (Exception e)
            {
                isCrashError = true;
                if (message is IRequest request)
                {
                    response = new ErrorResponse() { Id = request.Id, Code = (int)NetworkCode.InternalError, Message = e.Message };
                }
                log.Error(e.ToString());
#if DEBUG
                throw;
#endif
            }
            SendReponse(response, session);

            foreach (IFilter filter in chainFilters)
            {
                filter.After(message, response, session);
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

        public void SendReponse(IResponse response, ISession session)
        {
            if (response != null)
            {
                session.Connection.Send(response);
            }
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
