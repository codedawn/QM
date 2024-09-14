using DotNettyRPC;
using System;

namespace QM
{
    public class SessionFactory : ISessionFactory
    {
        public ISession CreateSession(IConnection connection)
        {
            Session session = new Session(connection.Cid, connection);
            connection.Session = session;
            return session;
        }

        public RemoteSession CreateRemoteSession(NetSession netSession)
        {
            RemoteSession remoteSession = new RemoteSession(netSession.Sid, netSession.ServerId, netSession.Data);
            return remoteSession;
        }

        public NetSession CreateNetSession(ISession session)
        {
            NetSession netSession = new NetSession() { Sid = session.Sid, ServerId = Application.current.serverId, Data = session.Data };
            return netSession;
        }

        public ISession SyncSession(NetSession netSession, ISession session)
        {
            session.Data = netSession.Data;
            return session;
        }
    }
}
