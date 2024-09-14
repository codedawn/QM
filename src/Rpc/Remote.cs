using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QM
{
    public class Remote : IRemote
    {
        private ServerComp _serverComp;
        private ILog _log = new NLogger(typeof(Remote));
        private ISessionFactory _sessionFactory;

        public Remote()
        {
            _serverComp = Application.current.GetComponent<ServerComp>();
            _sessionFactory = Application.current.GetSessionFactory();
        }

        //Connector执行这个方法，Server请求
        public Task Broadcast(IPush push)
        {
            if (!Application.current.isConnector) throw new QMException(ErrorCode.RPCNotAllowed, $"ServerType:{Application.current.serverType}当前服务器不允许执行该方法");
            List<ISession> sessions = Application.current.GetComponent<SessionComp>().GetAll();
            Task[] tasks = new Task[sessions.Count];
            for (int i = 0; i < sessions.Count; i++)
            {
                tasks[i] = sessions[i].Send(push);
            }
            return Task.WhenAll(tasks);
        }

        public Task BroadcastBySid(IPush push, List<string> sids)
        {
            if (!Application.current.isConnector) throw new QMException(ErrorCode.RPCNotAllowed, $"ServerType:{Application.current.serverType}当前服务器不允许执行该方法");
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < sids.Count; i++)
            {
                ISession session = Application.current.GetComponent<SessionComp>().Get(sids[i]);
                if (session != null)
                {
                    tasks.Add(session.Send(push));
                }
            }
            return Task.WhenAll(tasks);
        }

        //Server执行这个方法，Connector请求
        public async Task<IResponse> Forward(IMessage message, NetSession netSession)
        {
            if (Application.current.isConnector) throw new QMException(ErrorCode.RPCNotAllowed, $"ServerType:{Application.current.serverType}当前服务器不允许执行该方法");
            RemoteSession remoteSession = _sessionFactory.CreateRemoteSession(netSession);
            await _serverComp.GlobalHandleAsync(message, remoteSession);
            return remoteSession.Response;
        }

        //Connector执行这个方法，Server请求
        public Task Push(IPush push, string sid)
        {
            if (!Application.current.isConnector) throw new QMException(ErrorCode.RPCNotAllowed, $"ServerType:{Application.current.serverType}当前服务器不允许执行该方法");
            ISession session = Application.current.GetComponent<SessionComp>().Get(sid);
            if (session == null)
            {
                _log.Warn($"RPC调用Push时没有找到Sid:{sid}");
                return Task.CompletedTask;
            }
            return session.Send(push); ;
        }

        //Connector执行这个方法，Server请求
        public Task SyncSession(NetSession netSession)
        {
            if (!Application.current.isConnector) throw new QMException(ErrorCode.RPCNotAllowed, $"ServerType:{Application.current.serverType}当前服务器不允许执行该方法");
            ISession session = Application.current.GetComponent<SessionComp>().Get(netSession.Sid);
            if (session == null)
            {
                _log.Warn($"RPC调用SyncSession时没有找到Sid:{netSession.Sid}");
                return Task.CompletedTask;
            }
            _sessionFactory.SyncSession(netSession, session);
            return Task.CompletedTask;
        }

        public Task SessionClose(string sid)
        {
            if (!Application.current.isConnector) throw new QMException(ErrorCode.RPCNotAllowed, $"ServerType:{Application.current.serverType}当前服务器不允许执行该方法");
            ISession session = Application.current.GetComponent<SessionComp>().Get(sid);
            if (session == null)
            {
                _log.Warn($"RPC调用SessionClose时没有找到Sid:{sid}");
                return Task.CompletedTask;
            }
            session.Close();
            return Task.CompletedTask;
        }
    }
}

