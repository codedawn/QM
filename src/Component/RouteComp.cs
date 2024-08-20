using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class RouteComp : Component
    {
        private readonly Application _application;
        private Dictionary<string, List<string>> _serverTypes = new();
        private Dictionary<string, IPEndPoint> _serverAddr = new();
        public RouteComp(Application application)
        {
            _application = application;
        }

        public IPEndPoint Route(string serverType)
        {
            if (_serverTypes.TryGetValue(serverType, out List<string> result))
            {
                return _serverAddr[result[0]];
            }
            throw new Exception($"路由时找不到对应的serverType:{serverType}");
        }

        public IPEndPoint GetAddress(string serverId)
        {
            if (_serverAddr.TryGetValue(serverId, out IPEndPoint iPEndPoint))
            {
                return iPEndPoint;
            }
            return null;
        }

        public void UpdateServer(Dictionary<string, List<string>> serverTypes, Dictionary<string, IPEndPoint> servers)
        {
            _serverTypes = serverTypes;
            _serverAddr = servers;
        }
    }
}
