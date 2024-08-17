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
        private ConcurrentDictionary<string, IConnection> connections = new();

        public void Add(string address, IConnection connection)
        {
            if (connections.TryAdd(address, connection))
            {
                return;
            }
            else
            {
                //todo existed
            }
        }

        public void Remove(string address)
        {
            if (connections.TryRemove(address, out IConnection connection))
            {

            }
        }

        public int GetConnectionCount()
        {
            return connections.Count;
        }
    }
}
