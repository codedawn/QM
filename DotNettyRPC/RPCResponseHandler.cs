using QM;
using System.Collections.Generic;
using System.Diagnostics;

namespace DotNettyRPC
{
    internal class RPCResponseHandler<T>
    {
        private IAwait<T> _rpcWait { get; }

        public RPCResponseHandler(IAwait<T> rpcWait)
        {
            _rpcWait = rpcWait;
        }

        public void Handle(byte[] bytes)
        {
            RPCResponse responseModel = MessagePackUtil.Deserialize<RPCResponse>(bytes);
            T result = default;
            if (responseModel.Success)
            {
                if (responseModel.DataIndex != -1)
                {
                    result = (T)MessagePackUtil.Deserialize(MessageOpcodeHelper.GetType(responseModel.DataIndex), responseModel.Data);
                }
            }
            else
            {
                throw new QMException(ErrorCode.RPCServerError, $"RPC服务器异常，错误消息：{responseModel.Msg}");
            }
            _rpcWait.Set(responseModel.Id, result);
        }
    }
}
