using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace QM
{
    public class RpcComp : Component
    {
        private ILog _log = new NLogger(typeof(RpcComp));
        private RpcServer _rpcServer;
        private RpcClient _rpcClient;
        private Application _application;
        private RouteComp _routeComp;
        private ISessionFactory _sessionFactory;

        public RpcComp(Application application)
        {
            _application = application;

            //补充rpc中的MessageIndex
            MessageOpcode.Instance.AddMessageIndex(1000, typeof(List<string>));
            MessageOpcode.Instance.AddMessageIndex(1001, typeof(string));
        }

        public override void Start()
        {
            _rpcServer = new RpcServer();
            _rpcClient = new RpcClient();
            _routeComp = _application.GetComponent<RouteComp>();
            _sessionFactory = _application.GetSessionFactory();
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
            NetSession netSession = _sessionFactory.CreateNetSession(session);
            return await _rpcClient.Forward(message, netSession, iPEndPoint);
        }

        public async Task PushToConnector(IPush push, string serverId, string sid)
        {
            IPEndPoint iPEndPoint = _routeComp.GetConnector(serverId);
            if (iPEndPoint == null)
            {
                _log.Error($"Push消息时没有找到对应的serverId:{serverId}");
                return;
            }
            await _rpcClient.Push(push, sid, iPEndPoint);
        }

        /// <summary>
        /// 广播给所有session
        /// </summary>
        /// <param name="message"></param>
        public async Task Broadcast(IPush push)
        {
            List<IPEndPoint> iPEndPoints = _routeComp.GetAddresses(Application.Connector);
            Task[] tasks = new Task[iPEndPoints.Count];
            int i = 0;
            foreach (IPEndPoint iPEndPoint in iPEndPoints)
            {
                tasks[i++] = _rpcClient.Broadcast(push, iPEndPoint);
            }
            await Task.WhenAll(tasks);
        }

        public async Task BroadcastBySid(IPush push, List<string> sids)
        {
            List<IPEndPoint> iPEndPoints = _routeComp.GetAddresses(Application.Connector);
            Task[] tasks = new Task[iPEndPoints.Count];
            int i = 0;
            foreach (IPEndPoint iPEndPoint in iPEndPoints)
            {
                tasks[i++] = _rpcClient.BroadcastBySid(push, sids, iPEndPoint);
            }
            await Task.WhenAll(tasks);
        }

        public async Task SyncSessionToConnector(RemoteSession remoteSession)
        {
            NetSession netSession = _sessionFactory.CreateNetSession(remoteSession);
            netSession.TmpSid = remoteSession.TmpSid;
            IPEndPoint iPEndPoint = _routeComp.GetConnector(remoteSession.ServerId);
            if (iPEndPoint == null)
            {
                _log.Error($"Push消息时没有找到对应的serverId:{remoteSession.ServerId}");
                return;
            }
            await _rpcClient.SyncSession(netSession, iPEndPoint);
        }

        public async Task SessionCloseToConnector(RemoteSession remoteSession)
        {
            IPEndPoint iPEndPoint = _routeComp.GetConnector(remoteSession.ServerId);
            if (iPEndPoint == null)
            {
                _log.Error($"SessionSync消息时没有找到对应的serverId:{remoteSession.ServerId}");
                return;
            }
            await _rpcClient.SessionClose(remoteSession.Sid, iPEndPoint);
        }
    }
}
