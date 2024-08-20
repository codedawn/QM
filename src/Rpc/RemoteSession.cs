using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class RemoteSession : ISession
    {
        public string Sid { get; set; }
        public IConnection Connection { get; set; }
        public string serverId { get; set; }

        public RemoteSession(string sid, IConnection connection, string serverId)
        {
            Sid = sid;
            Connection = connection;
            this.serverId = serverId;   
        }
    }
}
