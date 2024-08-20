using System.Net;

namespace QM
{
    public class RpcComp : Component
    {
        private ILog _log = new ConsoleLog();
        private RpcServer _rpcServer;
        private RpcClient _rpcClient;
        private Application _application;
        private RouteComp _routeComp;

        public RpcComp(Application application)
        {
            _application = application;
        }

        public override void Start()
        {
            _rpcServer = new RpcServer();
            _rpcClient = new RpcClient();
            _routeComp = _application.GetComponent<RouteComp>();
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

        public IResponse Forward(IMessage message, ISession session, RouteInfo routeInfo)
        {
            IPEndPoint iPEndPoint = _routeComp.Route(routeInfo.ServerType);
            NetSession netSession = NetSession.Create((Session)session, Application.current.serverId);
            return  _rpcClient.Forward(message, netSession, iPEndPoint);
        }

        public void Push(IMessage message, string serverId, string sid)
        {
            IPEndPoint iPEndPoint = _routeComp.GetAddress(serverId);
            if (iPEndPoint == null)
            {
                _log.Error($"Push消息时没有找到对应的serverId:{serverId}");
                return;
            }
            NetSession netSession = new NetSession(sid, Application.current.serverId);
            _rpcClient.Push(message, netSession, iPEndPoint);
        }
    }
}
