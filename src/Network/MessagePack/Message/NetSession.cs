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

        public static NetSession Create(Session session)
        {
            NetSession netSession = new NetSession()
            {
                sid = session.sid
            };

            return netSession;
        }
    }
}
