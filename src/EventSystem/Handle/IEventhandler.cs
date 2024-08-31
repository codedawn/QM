using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface IEventHandler
    {
        public Task Handle(IEvent e);

        public Type GetEventType();
    }
}
