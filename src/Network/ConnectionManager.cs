using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, IConnection> _connections = new();

        public void Add(string address, IConnection connection)
        {
            _connections[address] = connection;
        }

        public IConnection Remove(string address)
        {
            if (_connections.TryRemove(address, out IConnection connection))
            {
                return connection;
            }
            return null;
        }

        public bool Exists(string address)
        {
            return _connections.ContainsKey(address);
        }

        public bool TryGetValue(string address, out IConnection connection)
        {
            return _connections.TryGetValue(address, out connection);
        }

        public int GetConnectionCount()
        {
            return _connections.Count;
        }
    }
}
