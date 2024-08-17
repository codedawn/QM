using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public abstract class EventHandler<T> : IEventHandler where T : IEvent
    {
        public Type GetEventType()
        {
            return typeof(T);
        }
        public void Handle(IEvent e)
        {
            Run((T)e);
        }

        public abstract void Run(T e);

    }
}
