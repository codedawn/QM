using QM.Log;
using QM.Network;

namespace QM.Component
{
    public class Server : IComponent
    {
        private ComponentState componentState;
        private List<IChainFilter> chainFilters;
        private ILog log;
        private MessageDispatcher dispatcher;

        public Server()
        {
            chainFilters = new List<IChainFilter>();
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


        public void GlobalHandle(Request request)
        {
            if (componentState != ComponentState.AfterStart)
            {
                log.Error("Server compoent状态不是AfterStart,不能处理信息");
                return;
            }

            bool isBreak = false;
            foreach (IChainFilter filter in chainFilters)
            {
                if (!filter.Before(request))
                {
                    isBreak = true;
                    break;
                }
            }

            if (isBreak)
            {
                return;
            }
            var response = dispatcher.Dispatch(request);
            SendReponse(response);

            foreach (IChainFilter filter in chainFilters)
            {
                filter.After(request, response);
            }
        }

        public void SendReponse(Response response)
        {

        }

        public void Stop()
        {
            componentState = ComponentState.Stop;
        }
    }
}
