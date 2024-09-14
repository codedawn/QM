using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    [MessagePackObject]
    [MessageIndex(333)]
    public class UserData : SessionData
    {
        [Key(0)]
        public bool IsAuth { get; set; }
    }
}
