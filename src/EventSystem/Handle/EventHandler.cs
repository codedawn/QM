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
        public async Task Handle(IEvent e)
        {
            await Run((T)e);
        }

        public abstract Task Run(T e);

    }
}
