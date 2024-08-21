using DotNettyRPC.Helper;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Coldairarrow.DotNettyRPC
{
    class ClientWait
    {
        private ConcurrentDictionary<long, ClientObj> _waits { get; set; } = new ConcurrentDictionary<long, ClientObj>();

        public void Start(long id)
        {
            ClientObj clientObj = new ClientObj();
            _waits[id] = clientObj;
        }

        public void Set(byte[] bytes)
        {
            ResponseModel responseModel = MessagePackUtil.Deserialize<ResponseModel>(bytes);
            ClientObj clientObj = _waits[responseModel.Id];
            if (clientObj == null) return;

            object result = null;
            if (responseModel.Success)
            {
                if (responseModel.DataIndex == -1)
                {
                    result = null;
                }
                else
                {
                    result = MessagePackUtil.Deserialize(MessageOpcodeHelper.GetType(responseModel.DataIndex), responseModel.Data);
                }
            }
            else
            {
                throw new Exception($"RPC服务器异常，错误消息：{responseModel.Msg}");
            }
            clientObj.tcs.SetResult(result);
            _waits.TryRemove(responseModel.Id, out ClientObj value);
        }

        public Task<object> Get(long id)
        {
            var clientObj = _waits[id];
            return clientObj.tcs.Task;
        }
    }
}