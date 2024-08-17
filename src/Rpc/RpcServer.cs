using Coldairarrow.DotNettyRPC;
using DotNettyRPC.Helper;

namespace QM
{
    public class RpcServer
    {
        private int _port = 9999;

        public RpcServer()
        {
            MessageOpcodeHelper.SetMessageOpcode(new RpcMessageOpcode());
        }

        public void Start()
        {
            RPCServer rpcServer = new RPCServer(_port);
            rpcServer.RegisterService<IRemote, Remote>();
            rpcServer.Start();
        }
    }
}
