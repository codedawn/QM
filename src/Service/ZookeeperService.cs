using org.apache.zookeeper;
using System.Text;
using static org.apache.zookeeper.ZooDefs;

namespace QM
{
    public class ZookeeperService
    {
        private static readonly string _address = "127.0.0.1:2181";
        private static readonly int _sessionTimeout = 5000;
        private static readonly string _servicePath = "/service";
        public Action OnWatch;
        private ZooKeeper _zookeeper;

        public async Task StartAsync()
        {
            if (_zookeeper != null)
            {
                await _zookeeper.closeAsync();
            }
            _zookeeper = new ZooKeeper(_address, _sessionTimeout, new MyWatcher(this));
            if (await _zookeeper.existsAsync(_servicePath, true) == null)
            {
                await _zookeeper.createAsync(_servicePath, null, Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT);
            }
            await WatchAsync();
        }

        public async Task WatchAsync()
        {
            await _zookeeper.getChildrenAsync(_servicePath, true);
        }

        public void Restart()
        {
            AsyncHelper.RunSync(() => { return StartAsync(); });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">/servertype:ip:port</param>
        /// <param name="idEndPoint">ip:port</param>
        /// <returns></returns>
        public async Task RegisterAsync(string node, string idEndPoint)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(idEndPoint);
            string path = _servicePath + node;
            if (await _zookeeper.existsAsync(path) == null)
            {
                await _zookeeper.createAsync(path, bytes, Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL);
            }
        }

        public async Task UnregisterAsync(string node)
        {
            string path = _servicePath + node;
            if (await _zookeeper.existsAsync(path) != null)
            {
                await _zookeeper.deleteAsync(path);
            }
        }

        public async Task<List<string>> GetServersAsync()
        {
            ChildrenResult childrenResult = await _zookeeper.getChildrenAsync(_servicePath, true);
            List<string> servers = new List<string>();
            foreach (var child in childrenResult.Children)
            {
                servers.Add(child);
            }
            return servers;
        }

        public void Stop()
        {
            if (_zookeeper != null)
            {
                _zookeeper.closeAsync();
            }
        }
    }

    internal class MyWatcher : Watcher
    {
        private ZookeeperService _service;
        public MyWatcher(ZookeeperService service)
        {
            _service = service;
        }

        public override async Task process(WatchedEvent @event)
        {
            Console.WriteLine(@event);
            var state = @event.getState();
            var type = @event.get_Type();

            if (state == Event.KeeperState.Expired)
            {
                _service.Restart();
            }

            if (type != Event.EventType.None)
            {
                _service.OnWatch?.Invoke();
                await _service.WatchAsync();
            }
        }
    }
}
