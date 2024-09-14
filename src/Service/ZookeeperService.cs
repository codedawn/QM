using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static org.apache.zookeeper.ZooDefs;

namespace QM
{
    public class ZookeeperService
    {
        private ILog _log = new ConsoleLogger();
        private static readonly string _address = "127.0.0.1:2182";
        private static readonly string _servicePath = "/service";

        private static readonly int _sessionTimeout = 5000;
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

        public async Task Restart()
        {
            await StartAsync();
        }

        /// <summary>
        /// node格式必须为/servertype:servername:ip:port
        /// </summary>
        /// <param name="nodePath">/servertype:servername:ip:port</param>
        /// <param name="data">ip:port</param>
        /// <returns></returns>
        public async Task RegisterAsync(string nodePath, string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            string path = _servicePath + nodePath;
            if (await _zookeeper.existsAsync(path) != null)
            {
                _log.Error($"注册失败，zookeeper已经注册path:{path}");
                return;
                //await _zookeeper.deleteAsync(path);
            }
            await _zookeeper.createAsync(path, bytes, Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL);
        }

        public async Task UnregisterAsync(string nodePath)
        {
            string path = _servicePath + nodePath;
            if (await _zookeeper.existsAsync(path) != null)
            {
                await _zookeeper.deleteAsync(path);
            }
        }

        public async Task<List<string>> GetServersAsync()
        {
            //如果外层使用AsyncHelper转同步这里会死锁
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
            var state = @event.getState();
            var type = @event.get_Type();

            if (state == Event.KeeperState.Expired)
            {
               await _service.Restart();
            }

            if (type != Event.EventType.None)
            {
                _service.OnWatch?.Invoke();
                await _service.WatchAsync();
            }
        }
    }
}
