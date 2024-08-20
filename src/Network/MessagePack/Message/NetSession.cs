using MessagePack;
using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    [MessageIndex(3)]
    [MessagePackObject]
    public class NetSession
    {
        [Key(0)]
        public string sid;

        [Key(1)]
        public string serverId;

        public static NetSession Create(Session session, string serverId)
        {
            NetSession netSession = new NetSession()
            {
                sid = session.Sid,
                serverId = serverId
            };

            return netSession;
        }

        public NetSession(string sid, string serverId)
        {
            this.sid = sid;
            this.serverId = serverId;
        }

        public NetSession()
        {
        }
    }
}
