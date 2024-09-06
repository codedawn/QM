using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class Session : ISession
    {
        public string Sid { get; set; }
        public IConnection Connection { get; set; }
        public long lastAccessTime;

        public Session(string sid, IConnection connection, long lastAccessTime)
        {
            this.Sid = sid;
            this.Connection = connection;
            this.lastAccessTime = lastAccessTime;
        }
    }
}
