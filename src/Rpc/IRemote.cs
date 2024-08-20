namespace QM
{
    public interface IRemote
    {
        public IResponse Forward(IMessage message, NetSession netSession);

        public void Push(IMessage message, NetSession netSession);
    }
}
