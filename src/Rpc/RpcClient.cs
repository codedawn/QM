using DotNettyRPC;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace QM
{
    public class RpcClient
    {
        private int _timeout = 100000;
        private ILog _log = new NLogger(typeof(RpcClient));
        public RpcClient()
        {
            MessageOpcodeHelper.SetMessageOpcode(new RpcMessageOpcode());
        }

        public async Task<IResponse> Forward(IMessage message, NetSession netSession, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote, IResponse>(iPEndPoint.Address.ToString(), iPEndPoint.Port, _timeout);
            return await client.Forward(message, netSession);
        }

        public async Task Push(IMessage message, NetSession netSession, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote, IResponse>(iPEndPoint.Address.ToString(), iPEndPoint.Port, _timeout);
            await client.Push(message, netSession);
        }

        public async Task Broadcast(IMessage message, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote, IResponse>(iPEndPoint.Address.ToString(), iPEndPoint.Port, _timeout);
            await client.Broadcast(message);
        }
    }
}
