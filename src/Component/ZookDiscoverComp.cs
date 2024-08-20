using System.Net;

namespace QM
{
    public class ZookDiscoverComp : Component
    {
        private ILog _log;
        private ZookeeperService _zookeeperService;
        private RouteComp _routeComp;
        private Application _application;

        public ZookDiscoverComp(Application application)
        {
            _application = application;
            _log = new ConsoleLog();
        }

        public override void Start()
        {
            _zookeeperService = new ZookeeperService();
            _routeComp = _application.GetComponent<RouteComp>();

            _zookeeperService.OnWatch += RefreshServerInfo;
            AsyncHelper.RunSync(() => _zookeeperService.StartAsync());
            AsyncHelper.RunSync(() => _zookeeperService.RegisterAsync($"/{Application.current.serverType}:{Application.current.serverId}:127.0.0.1:" + Application.current.rpcPort, "127.0.0.1:29999"));
            base.Start();
        }

        public override void Stop()
        {
            _zookeeperService?.Stop();

            base.Stop();
        }

        public async void RefreshServerInfo()
        {
            var serverTypes = new Dictionary<string, List<string>>();
            var serverAddrs = new Dictionary<string, IPEndPoint>();
            // servertype:servername:ip:port
            List<string> servers = await _zookeeperService.GetServersAsync();

            foreach (var server in servers)
            {
                var args = server.Split(':');
                if (args.Length != 4) continue;

                if (!IPAddress.TryParse(args[2], out var ipAddress) || !int.TryParse(args[3], out var port))
                {
                    continue;
                }

                string serverId = args[1];
                IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

                if (serverTypes.TryGetValue(args[0], out var serverIds))
                {
                    serverIds.Add(serverId);
                }
                else
                {
                    serverTypes[args[0]] = new List<string> { serverId };
                }
                serverAddrs.Add(serverId, endPoint);
            }

            _routeComp.UpdateServer(serverTypes, serverAddrs);
            _log.Info("RefreshServerInfo");
        }
    }
}
