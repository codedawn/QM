﻿namespace QM
{
    public class Remote : IRemote
    {
        public Task<object> Forward(IMessage message, NetSession netSession)
        {
            RemoteConnection remoteConnection = new RemoteConnection();
            RemoteSession remoteSession = new RemoteSession(netSession.sid, remoteConnection, netSession.serverId);

            //Application.current.GetComponent<ServerComp>().GlobalHandle(message, remoteSession);
            remoteConnection.Send(new UserResponse() { Id = 42142132131, Name = "fkewfo" });
            return Task.FromResult((object)remoteConnection.response);
        }

        public void Push(IMessage message, NetSession netSession)
        {
            ISession session = Application.current.GetComponent<SessionComp>().Get(netSession.sid);
            if (session == null)
            {
                return;
            }
            session.Connection.Send(message);
        }

        public async Task<object> Test(IMessage message, NetSession netSession)
        {
            RemoteConnection remoteConnection = new RemoteConnection();
            RemoteSession remoteSession = new RemoteSession(netSession.sid, remoteConnection, netSession.serverId);

            //Application.current.GetComponent<ServerComp>().GlobalHandle(message, remoteSession);
            remoteConnection.Send(new UserResponse() { Id = 42142132131, Name = "fkewfo" });
            return remoteConnection.response;
        }

    }
}

