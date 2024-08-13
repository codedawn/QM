using QM.Log;
using QM.Network;

namespace QM.Component
{
    public class Server : IComponent
    {
        private ComponentState componentState;
        private List<IChainFilter> chainFilters;
        private ILog log;
        private MessageDispatcher _dispatcher;
        private Application _application;

        public Server(Application application)
        {
            chainFilters = new List<IChainFilter>();
            _dispatcher = new MessageDispatcher(_application);
            componentState = ComponentState.Init;
        }

        public void Start()
        {
            componentState = ComponentState.Start;
        }

        public void AfterStart()
        {
            componentState = ComponentState.AfterStart;
        }


        public void GlobalHandle(IMessage message, Session session)
        {
            if (componentState != ComponentState.AfterStart)
            {
                log.Error("Server compoent状态不是AfterStart,不能处理信息");
                return;
            }

            if (message is not IRequest)
            {
                log.Error($"收到来自客户端的非Request消息message:{message.ToString()}");
                return;
            }

            IRequest request = (IRequest)message;
            bool isFilter = false;
            foreach (IChainFilter filter in chainFilters)
            {
                if (!filter.Before(request))
                {
                    isFilter = true;
                    break;
                }
            }

            //before可以拦截消息，不会继续传播
            if (isFilter)
            {
                return;
            }
            IResponse response = _dispatcher.Dispatch(request, session);
            SendReponse(response);

            foreach (IChainFilter filter in chainFilters)
            {
                filter.After(request, response);
            }
        }

        public void SendReponse(IResponse response)
        {

        }

        public void Stop()
        {
            componentState = ComponentState.Stop;
        }
    }
}
