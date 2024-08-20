using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class UserAddEvent : IEvent
    {
        public RemoteSession Session { get; set; }
    }
}
