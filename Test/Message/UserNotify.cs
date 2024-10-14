using MessagePack;
using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [MessageIndex(10)]
    [MessagePackObject]
    [MessageDispatch(ServerType.Server)]
    public class UserNotify : INotify
    {
        [Key(0)]
        public string message;
    }
}
