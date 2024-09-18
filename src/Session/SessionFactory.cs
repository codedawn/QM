using DotNettyRPC;
using System;

namespace QM
{
    public class SessionFactory : ISessionFactory
    {
        public ISession CreateSession(IConnection connection)
        {
            string sid = connection.Cid;
            SessionComp sessionComp = Application.current.GetComponent<SessionComp>();
            Session session = (Session)sessionComp.Get(sid);
            if (session == null)
            {
                session = new Session(sid, connection);
            }
            else
            {
                session.Connection = connection;
            }

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
            //修改了sid
            if (netSession.TmpSid != netSession.Sid)
            {
                session.Sid = netSession.TmpSid;
                SessionComp sessionComp = Application.current.GetComponent<SessionComp>();
                Session rawSession = (Session)sessionComp.Get(netSession.TmpSid);
                //如果要更新的Sid已经存在Session，就保留旧的Sid
                //相当于重新登录，新的连接顶替旧连接
                if (rawSession != null)
                {
                    rawSession.Connection = ((Session)session).Connection;
                }
                else
                {
                    sessionComp.AddOrUpdate(netSession.TmpSid, session);
                }
                sessionComp.Remove(netSession.Sid);
            }
            session.Data = netSession.Data;
            return session;
        }
    }
}
