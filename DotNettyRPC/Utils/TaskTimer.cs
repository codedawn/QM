using DotNetty.Common.Utilities;
using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyRPC
{
    public class TaskTimer : ITaskTimer
    {
        private HashedWheelTimer _timer = new HashedWheelTimer();

        public void schedule(Action<long> action, long args, int timeout)
        {
            _timer.NewTimeout(new ActionTimerTask(t => { action?.Invoke(args); }), TimeSpan.FromMilliseconds(timeout));
        }

        public void schedule(Action action, int timeout)
        {
            _timer.NewTimeout(new ActionTimerTask(t => { action?.Invoke(); }), TimeSpan.FromMilliseconds(timeout));
        }
    }
}
