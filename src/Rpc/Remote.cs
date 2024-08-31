namespace QM
{
    public class Remote : IRemote
    {
        public Task Broadcast(IMessage message)
        {
            List<ISession> sessions = Application.current.GetComponent<SessionComp>().GetAll();
            foreach (var session in sessions)
            {
                session.Connection.Send(message);
            }
            return Task.CompletedTask;
        }

        public async Task<IResponse> Forward(IMessage message, NetSession netSession)
        {
            RemoteConnection remoteConnection = new RemoteConnection();
            RemoteSession remoteSession = new RemoteSession(netSession.sid, remoteConnection, netSession.serverId);

            await Application.current.GetComponent<ServerComp>().GlobalHandleAsync(message, remoteSession);
            //remoteConnection.Send(new UserResponse() { Id = 42142132131, Name = "fkewfo" });
            return remoteConnection.response;
        }

        public Task Push(IMessage message, NetSession netSession)
        {
            ISession session = Application.current.GetComponent<SessionComp>().Get(netSession.sid);
            if (session == null)
            {
                return Task.CompletedTask;
            }
            session.Connection.Send(message);
            return Task.CompletedTask;
        }

        public async Task<IResponse> Test(IMessage message, NetSession netSession)
        {
            RemoteConnection remoteConnection = new RemoteConnection();
            RemoteSession remoteSession = new RemoteSession(netSession.sid, remoteConnection, netSession.serverId);

            //Application.current.GetComponent<ServerComp>().GlobalHandle(message, remoteSession);
            remoteConnection.Send(new UserResponse() { Id = 42142132131, Name = "fkewfo" });
            return remoteConnection.response;
        }

    }
}

