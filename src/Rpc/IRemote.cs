using System.Threading.Tasks;

namespace QM
{
    public interface IRemote
    {
        public Task<IResponse> Forward(IMessage message, NetSession netSession);

        public Task Push(IMessage message, NetSession netSession);
        public Task Broadcast(IMessage message);
    }
}
