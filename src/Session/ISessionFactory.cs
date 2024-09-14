using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface ISessionFactory
    {
        public ISession CreateSession(IConnection connection);
        public RemoteSession CreateRemoteSession(NetSession netSession);
        public NetSession CreateNetSession(ISession session);
        public ISession SyncSession(NetSession netSession, ISession session);
    }
}
