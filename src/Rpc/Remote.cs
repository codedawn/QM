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

        public Remote()
        {
            _serverComp = Application.current.GetComponent<ServerComp>();
        }

        //Connector执行，Server请求
        public Task Broadcast(IMessage message)
        {
            List<ISession> sessions = Application.current.GetComponent<SessionComp>().GetAll();
            Task[] tasks = new Task[sessions.Count];
            for (int i = 0; i < sessions.Count; i++)
            {
                tasks[i] = sessions[i].Connection.Send(message);
            }
            return Task.WhenAll(tasks);
        }

        //Server执行，Connector请求
        public async Task<IResponse> Forward(IMessage message, NetSession netSession)
        {
            RemoteConnection remoteConnection = new RemoteConnection();
            RemoteSession remoteSession = new RemoteSession(netSession.sid, remoteConnection, netSession.serverId);
            await _serverComp.GlobalHandleAsync(message, remoteSession);
            return remoteConnection.response;
        }

        //Connector执行，Server请求
        public Task Push(IMessage message, NetSession netSession)
        {
            ISession session = Application.current.GetComponent<SessionComp>().Get(netSession.sid);
            if (session == null)
            {
                return Task.CompletedTask;
            }
            return session.Connection.Send(message);;
        }
    }
}

