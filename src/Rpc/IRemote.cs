namespace QM
{
    public interface IRemote
    {
        public IResponse Forward(IMessage message, NetSession netSession);
    }
}
