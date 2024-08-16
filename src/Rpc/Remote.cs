using QM.Network;

namespace QM.Rpc
{
    public class Remote : IRemote
    {
        public IResponse Forward(IMessage message, NetSession netSession)
        {

            Session session = new Session() { sid = netSession.sid };

            RemoteConnection remoteConnection = new RemoteConnection();
            session.connection = remoteConnection;
            //DoSomething(message, session);
            remoteConnection.Send(new UserResponse() { Id = 42142132131, Name = "fkewfo" });
            return (IResponse)remoteConnection.response;
        }
    }

    public class RemoteConnection : IConnection
    {
        public IMessage response;
        public string Address => throw new NotImplementedException();

        public string Cid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Send(IMessage message)
        {
            response = message;
        }
    }
}

