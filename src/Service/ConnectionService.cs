using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Service
{
    /**
     * 维护连接数等信息
     */
    public class ConnectionService
    {
        private int connCount;

        public ConnectionService()
        {
            connCount = 0;
        }

        public void addConnection()
        {
            connCount++;
        }

        public void removeConnection()
        {
            connCount--;
        }

        public int getConnectionCount()
        {
            return connCount;
        }
    }
}
