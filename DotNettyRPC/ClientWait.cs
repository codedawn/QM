using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DotNettyRPC
{
    class ClientWait<IResult> : IClientWait
    {
        private int _timeout;
        private ConcurrentDictionary<long, TaskCompletionSource<IResult>> _waits { get; set; } = new();
        private TaskTimer _timer = new TaskTimer();

        public ClientWait(int timeout)
        {
            _timeout = timeout;
        }

        public void Start(long id)
        {
            _timer.schedule((id) => Timeout(id), id, _timeout);
            TaskCompletionSource<IResult> tcs = new TaskCompletionSource<IResult>();
            _waits[id] = tcs;
        }

        private void Timeout(long id)
        {
            if (_waits.TryRemove(id, out TaskCompletionSource<IResult> tcs))
            {
                tcs.SetException(new Exception("RPC调用超时"));
            }
        }

        public void Set(byte[] bytes)
        {
            ResponseModel responseModel = MessagePackUtil.Deserialize<ResponseModel>(bytes);
            if (!_waits.TryRemove(responseModel.Id, out TaskCompletionSource<IResult> tcs))
                return;

            IResult result = default(IResult);
            if (responseModel.Success)
            {
                if (responseModel.DataIndex != -1)
                {
                    result = (IResult)MessagePackUtil.Deserialize(MessageOpcodeHelper.GetType(responseModel.DataIndex), responseModel.Data);
                }
            }
            else
            {
                throw new Exception($"RPC服务器异常，错误消息：{responseModel.Msg}");
            }
            tcs.SetResult(result);
        }

        public object Get(long id)
        {
            var tcs = _waits[id];
            return tcs.Task;
        }
    }
}