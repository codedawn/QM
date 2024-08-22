using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DotNettyRPC
{
    class ClientWait
    {
        private int _timeout;
        private ConcurrentDictionary<long, ClientObj> _waits { get; set; } = new ConcurrentDictionary<long, ClientObj>();
        private TaskTimer _timer = new TaskTimer();

        public ClientWait(int timeout)
        {
            _timeout = timeout;
        }

        public void Start(long id)
        {
            _timer.schedule((id) => Timeout(id), id, _timeout);
            ClientObj clientObj = new ClientObj();
            _waits[id] = clientObj;
        }

        private void Timeout(long id)
        {
            if (_waits.TryRemove(id, out ClientObj clientObj))
            {
                clientObj.tcs.SetException(new Exception("RPC调用超时"));
            }
        }

        public void Set(byte[] bytes)
        {
            ResponseModel responseModel = MessagePackUtil.Deserialize<ResponseModel>(bytes);
            if (!_waits.TryRemove(responseModel.Id, out ClientObj clientObj))
                return;

            object result = null;
            if (responseModel.Success)
            {
                if (responseModel.DataIndex != -1)
                {
                    result = MessagePackUtil.Deserialize(MessageOpcodeHelper.GetType(responseModel.DataIndex), responseModel.Data);
                }
            }
            else
            {
                throw new Exception($"RPC服务器异常，错误消息：{responseModel.Msg}");
            }
            clientObj.tcs.SetResult(result);
        }

        public Task<object> Get(long id)
        {
            var clientObj = _waits[id];
            return clientObj.tcs.Task;
        }
    }
}