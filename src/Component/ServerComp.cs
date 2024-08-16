using QM.Log;
using QM.Network;
using QM.Service;
using Src.Network;
using static System.Net.Mime.MediaTypeNames;

namespace QM.Component
{
    public class ServerComp : Component
    {
        private List<IChainFilter> chainFilters;
        private ILog log;
        private MessageDispatcher _messageDispatcher;
        private Application _application;
        private ServerDispatcher _serverDispatcher;

        public ServerComp(Application application)
        {
            _application = application;
            chainFilters = new List<IChainFilter>();
            log = new ConsoleLog();
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


        public void GlobalHandle(IMessage message, Session session)
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

            IRequest request = (IRequest)message;
            IResponse response = null;
            bool isFilter = false;
            bool isCrashError = false;
            try
            {
                foreach (IChainFilter filter in chainFilters)
                {
                    if (!filter.Before(request))
                    {
                        isFilter = true;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                isCrashError = true;
                response = new ErrorResponse() { Id = request.Id, Code = (int)NetworkCode.InternalError, Message = e.Message };
                log.Error(e.ToString());
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
                response = _messageDispatcher.Dispatch(request, session, routeInfo);
            }
            catch (Exception e)
            {
                isCrashError = true;
                response = new ErrorResponse() { Id = request.Id, Code = (int)NetworkCode.InternalError, Message = e.Message };
                log.Error(e.ToString());
            }
            SendReponse(response, session);

            foreach (IChainFilter filter in chainFilters)
            {
                filter.After(request, response);
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

        public void SendReponse(IResponse response, Session session)
        {
            if (response != null)
            {
                session.connection.Send(response);
            }
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
