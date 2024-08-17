using System.Net;

namespace QM
{
    public class ZookDiscoverComp : Component
    {
        private ZookeeperService _zookeeperService;
        private LoadBalanceComp _loadBalanceComp;
        private Application _application;

        public ZookDiscoverComp(Application application)
        {
            _application = application;
        }

        public override void Start()
        {
            _zookeeperService = new ZookeeperService();
            _loadBalanceComp = _application.GetComponent<LoadBalanceComp>();

            _zookeeperService.OnWatch += RefreshServerInfo;
            AsyncHelper.RunSync(async () =>
                {
                    await _zookeeperService.StartAsync();
                    return _zookeeperService.RegisterAsync("/server:127.0.0.1:29999", "127.0.0.1:29999");
                });

            base.Start();
        }

        public override void Stop()
        {
            _zookeeperService?.Stop();

            base.Stop();
        }

        public void RefreshServerInfo()
        {
            var serverAddress = new Dictionary<string, List<IPEndPoint>>();
            List<string> servers = AsyncHelper.RunSync(() => { return _zookeeperService.GetServersAsync(); });

            foreach (var server in servers)
            {
                var args = server.Split(':');
                if (args.Length != 3) continue;

                if (!IPAddress.TryParse(args[1], out var ipAddress) || !int.TryParse(args[2], out var port))
                {
                    continue;
                }

                var endPoint = new IPEndPoint(ipAddress, port);

                if (serverAddress.TryGetValue(args[0], out var endPoints))
                {
                    endPoints.Add(endPoint);
                }
                else
                {
                    serverAddress[args[0]] = new List<IPEndPoint> { endPoint };
                }
            }

            _loadBalanceComp.UpdateServer(serverAddress);
        }
    }
}
