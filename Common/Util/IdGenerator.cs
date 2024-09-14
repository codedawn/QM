using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    public class IdGenerator
    {
        private static readonly IdWorker _idWorker = new IdWorker(1, 1);
        public static long NextId()
        {
            return _idWorker.NextId();
        }
    }
}
