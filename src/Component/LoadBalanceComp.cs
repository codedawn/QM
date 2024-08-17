using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class LoadBalanceComp : Component
    {
        private readonly Application _application;
        private Dictionary<string, List<IPEndPoint>> _serverAddress = new();
        //private Dictionary<string, List<IPEndPoint>> _
        public LoadBalanceComp(Application application)
        {
            _application = application;
        }

        public IPEndPoint Route(string server)
        {
            if (_serverAddress.TryGetValue(server, out List<IPEndPoint> result))
            {
                return result[0];
            }
            return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
        }

        public void UpdateServer(Dictionary<string, List<IPEndPoint>> servers)
        {
            _serverAddress = servers;
        }
    }
}
