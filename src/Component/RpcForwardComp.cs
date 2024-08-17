using System.Net;

namespace QM
{
    public class RpcForwardComp : Component
    {
        private RpcServer _rpcServer;
        private RpcClient _rpcClient;
        private Application _application;
        private LoadBalanceComp _loadBalance;

        public RpcForwardComp(Application application)
        {
            _application = application;
        }

        public override void Start()
        {
            _rpcServer = new RpcServer();
            _rpcClient = new RpcClient();
            _loadBalance = _application.GetComponent<LoadBalanceComp>();
            base.Start();
        }

        public override void AfterStart()
        {
            _rpcServer.Start();
            base.AfterStart();
        }

        public override void Stop()
        {
            base.Stop();
        }

        public IResponse Forward(IMessage message, Session session, RouteInfo routeInfo)
        {
            IPEndPoint iPEndPoint = _loadBalance.Route(routeInfo.ServerType);
            NetSession netSession = NetSession.Create(session);
            return  _rpcClient.Forward(message, session, iPEndPoint);
        }
    }
}
