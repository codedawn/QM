using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface ITaskTimer
    {
        public void schedule(Action action, int timeout);
    }
}
