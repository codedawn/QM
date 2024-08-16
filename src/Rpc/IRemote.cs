using QM.Network;

namespace QM.Rpc
{
    public interface IRemote
    {
        public IResponse Forward(IMessage message, NetSession netSession);
    }
}
