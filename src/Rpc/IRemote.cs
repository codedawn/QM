namespace QM
{
    public interface IRemote
    {
        public Task<object> Forward(IMessage message, NetSession netSession);

        public Task<object> Test(IMessage message, NetSession netSession);

        public void Push(IMessage message, NetSession netSession);
    }
}
