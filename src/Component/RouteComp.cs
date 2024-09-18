using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class RouteComp : Component
    {
        private ILog _log = new NLogger(typeof(RouteComp));
        private readonly Application _application;
        private Dictionary<string, List<ServerInfo>> _serverTypes = new Dictionary<string, List<ServerInfo>>();
        private Dictionary<string, IRouter> _routers = new Dictionary<string, IRouter>();
        private IRouter _defaultRouter = new DefaultRouter();

        public RouteComp(Application application)
        {
            _application = application;
            InitRoute();
        }

        private void InitRoute()
        {
            foreach (Type type in CodeType.Instance.GetTypes(typeof(RouteAttribute)))
            {
                IRouter router = Activator.CreateInstance(type) as IRouter;
                RouteAttribute routeAttribute = type.GetCustomAttribute<RouteAttribute>(); 
                if (!_routers.TryAdd(routeAttribute.serverType, router))
                {
                    _log.Error($"不能重复定义route，serverType:{routeAttribute.serverType}");
                }
            }
        }

        public IPEndPoint Route(string serverType)
        {
            if (_serverTypes.TryGetValue(serverType, out List<ServerInfo> result))
            {
                if (_routers.TryGetValue(serverType, out IRouter route))
                {
                    return route.Route(result);
                }

                return _defaultRouter.Route(result);
            }
            throw new QMException(ErrorCode.ServerTypeNoOne, $"找不到类型:{serverType}的机子");
        }

        public IPEndPoint GetConnector(string serverId)
        {
            if (_serverTypes.TryGetValue(Application.Connector, out List<ServerInfo> serverInfos))
            {
                foreach (var info in serverInfos)
                {
                    if (info.serverId == serverId)
                    {
                        return info.iPEndPoint;
                    }
                }
            }
            return null;
        }

        public List<IPEndPoint> GetAddresses(string serverType)
        {
            List<IPEndPoint> result = new List<IPEndPoint>();
            if (!_serverTypes.ContainsKey(serverType)) return result;

            foreach (ServerInfo serverInfo in _serverTypes[serverType])
            {
                result.Add(serverInfo.iPEndPoint);
            }
            return result;
        }

        public void UpdateServer(Dictionary<string, List<ServerInfo>> serverTypes)
        {
            _serverTypes = serverTypes;
        }
    }
}
