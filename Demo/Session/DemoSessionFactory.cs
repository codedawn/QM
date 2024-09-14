using DotNettyRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    public class DemoSessionFactory : ISessionFactory
    {
        private readonly ISessionFactory _sessionFactory = new SessionFactory();

        public NetSession CreateNetSession(ISession session)
        {
            return _sessionFactory.CreateNetSession(session);
        }

        public RemoteSession CreateRemoteSession(NetSession netSession)
        {
            RemoteSession remoteSession = _sessionFactory.CreateRemoteSession(netSession);
            return remoteSession;
        }

        public ISession CreateSession(IConnection connection)
        {
            ISession session = _sessionFactory.CreateSession(connection);
            session.Data = new UserData();
            return session;
        }

        public ISession SyncSession(NetSession netSession, ISession session)
        {
            return _sessionFactory.SyncSession(netSession, session);
        }
    }
}
