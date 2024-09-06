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
#if DEBUG
        private ConcurrentDictionary<long, Stopwatch> timeMeasurements = new ConcurrentDictionary<long, Stopwatch>();
#endif
        private int _timeout;
        private ConcurrentDictionary<long, TaskCompletionSource<T>> _waits { get; set; } = new ConcurrentDictionary<long, TaskCompletionSource<T>>();
        private ITaskTimer _taskTimer;
        private ILog _log = new NLogger(typeof(CommonAwait<T>));
        private string _name;

        public CommonAwait(int timeoutMS, ITaskTimer taskTimer, string name)
        {
            _timeout = timeoutMS;
            _taskTimer = taskTimer;
            _name = name;
        }

        public Task<T> Start(long id)
        {
#if DEBUG
            timeMeasurements.TryAdd(id, Stopwatch.StartNew());
#endif
            _taskTimer.schedule(() => Timeout(id), _timeout);
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
#if DEBUG
            if (timeMeasurements.TryGetValue(id, out Stopwatch stopwatch))
            {
                stopwatch.Stop();
                _log.Debug($"{_name}id:{id}到达耗时：{stopwatch.ElapsedMilliseconds}ms");
            }
             
#endif
            if (_waits.Remove(id, out TaskCompletionSource<T> tcs))
            {
                tcs.SetResult(result);
                return;
            }

            //throw new QMException(ErrorCode.AwaitNotFoundId, $"不存在id:{id}");
        }
    }
}