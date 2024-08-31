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

        public async Task<IResponse> ForwardToServer(IMessage message, ISession session, RouteInfo routeInfo)
        {
            IPEndPoint iPEndPoint = _routeComp.Route(routeInfo.ServerType);
            NetSession netSession = NetSession.Create((Session)session, Application.current.serverId);
            return  await _rpcClient.Forward(message, netSession, iPEndPoint);
        }

        public async Task PushToConnector(IMessage message, string serverId, string sid)
        {
            IPEndPoint iPEndPoint = _routeComp.GetAddress(serverId);
            if (iPEndPoint == null)
            {
                _log.Error($"Push消息时没有找到对应的serverId:{serverId}");
                return;
            }
            NetSession netSession = new NetSession(sid, Application.current.serverId);
            await _rpcClient.Push(message, netSession, iPEndPoint);
        }

        /// <summary>
        /// 广播给所有session
        /// </summary>
        /// <param name="message"></param>
        public async Task Broadcast(IMessage message)
        {
            List<IPEndPoint> iPEndPoints = _routeComp.GetAddresses(Application.Connector);
            Task[] tasks = new Task[iPEndPoints.Count];
            int i = 0;
            foreach(IPEndPoint iPEndPoint in iPEndPoints)
            {
                tasks[i++] = _rpcClient.Broadcast(message, iPEndPoint); 
            }
            await Task.WhenAll(tasks);
        }
    }
}
