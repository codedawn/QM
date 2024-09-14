using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace QM
{
    public class ZookeeperComp : Component
    {
        private ILog _log;
        private ZookeeperService _zookeeperService;
        private RouteComp _routeComp;
        private Application _application;


        public ZookeeperComp(Application application)
        {
            _application = application;
            _log = new ConsoleLogger();
        }

        public override void Start()
        {
            _zookeeperService = new ZookeeperService();
            _routeComp = _application.GetComponent<RouteComp>();

            _zookeeperService.OnWatch += async () =>
            {
                try
                {
                    await RefreshServerInfo();
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            };
            AsyncHelper.RunSync(() => _zookeeperService.StartAsync());
            AsyncHelper.RunSync(() => _zookeeperService.RegisterAsync(GetServerNodePath(), ""));
            base.Start();
        }

        public override void Stop()
        {
            _zookeeperService?.Stop();

            base.Stop();
        }

        public async Task RefreshServerInfo()
        {
            var serverTypes = new Dictionary<string, List<ServerInfo>>();
            List<string> servers = await _zookeeperService.GetServersAsync();

            // serverType:serverId:ip:port
            foreach (var server in servers)
            {
                var args = server.Split(':');
                if (args.Length != 4) continue;

                if (!IPAddress.TryParse(args[2], out var ipAddress) || !int.TryParse(args[3], out var port))
                {
                    continue;
                }

                string serverType = args[0];
                string serverId = args[1];
                IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

                if (serverTypes.TryGetValue(serverType, out var serverIds))
                {
                    serverIds.Add(new ServerInfo(endPoint, serverType, serverId, default));
                }
                else
                {
                    serverTypes[args[0]] = new List<ServerInfo> { new ServerInfo(endPoint, serverType, serverId, default) };
                }
            }

            _routeComp.UpdateServer(serverTypes);
            _log.Info("RefreshServerInfo");
        }

        public string GetServerNodePath()
        {
            return $"/{Application.current.serverType}:{Application.current.serverId}:127.0.0.1:" + Application.current.rpcPort;
        }
    }
}
