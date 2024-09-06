using Amib.Threading;
using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    public static class QMThreadPool
    {
        private static SmartThreadPool smartThreadPool = new SmartThreadPool();

        public static void QueueWorkItem(Action action)
        {
            smartThreadPool.QueueWorkItem(action);
        }
    }


}
