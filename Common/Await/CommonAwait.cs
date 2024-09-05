using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace QM
{
    public class CommonAwait<T> : IAwait<T>
    {
        private int _timeout;
        private ConcurrentDictionary<long, TaskCompletionSource<T>> _waits { get; set; } = new ConcurrentDictionary<long, TaskCompletionSource<T>>();
        private ITaskTimer _taskTimer;

        public CommonAwait(int timeoutMS, ITaskTimer taskTimer)
        {
            _timeout = timeoutMS;
            _taskTimer = taskTimer;
        }

        public Task<T> Start(long id)
        {
            _taskTimer.schedule((id) => Timeout(id), id, _timeout);
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            if (_waits.ContainsKey(id))
            {
                throw new QMException(ErrorCode.AwaitDupliId, $"插入已经存在id:{id}");
            }
            _waits[id] = tcs;
            return tcs.Task;
        }

        private void Timeout(long id)
        {
            if (_waits.TryRemove(id, out TaskCompletionSource<T> tcs))
            {
                tcs.SetException(new QMException(ErrorCode.AwaitTimeout, $"id:{id}Await超时"));
            }
        }

        public void Set(long id, T result)
        {
            if (_waits.Remove(id, out TaskCompletionSource<T> tcs))
            {
                tcs.SetResult(result);
                return;
            }

            //throw new QMException(ErrorCode.AwaitNotFoundId, $"不存在id:{id}");
        }
    }
}