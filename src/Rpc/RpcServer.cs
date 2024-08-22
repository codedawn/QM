using DotNettyRPC;

namespace QM
{
    public class RpcServer
    {
        public RpcServer()
        {
            MessageOpcodeHelper.SetMessageOpcode(new RpcMessageOpcode());
        }

        public void Start()
        {
            RPCServer rpcServer = new RPCServer(Application.current.rpcPort);
            rpcServer.RegisterService<IRemote, Remote>();
            rpcServer.Start();
        }
    }
}
