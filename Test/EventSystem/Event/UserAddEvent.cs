using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class UserAddEvent : IEvent
    {
        public RemoteSession Session { get; set; }
    }
}
