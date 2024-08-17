using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class Session
    {
        public string sid;
        public IConnection connection;
        public long lastAccessTime;

        public Session(string sid, IConnection connection, long lastAccessTime)
        {
            this.sid = sid;
            this.connection = connection;
            this.lastAccessTime = lastAccessTime;
        }
    }
}
