using System.Collections.Generic;
using System.Threading.Tasks;

namespace QM
{
    public interface IRemote
    {
        public Task<IResponse> Forward(IMessage message, NetSession netSession);
        public Task Push(IPush push, string sid);
        public Task Broadcast(IPush push);
        public Task BroadcastBySid(IPush push, List<string> sids);
        public Task SyncSession(NetSession netSession);
        public Task SessionClose(string sid);
    }
}
