using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface IEventHandler
    {
        public void Handle(IEvent e);

        public Type GetEventType();
    }
}
