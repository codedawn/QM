using MessagePack;
using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    [MessageIndex(101)]
    [MessagePackObject]
    public class UserJoinResponse : IResponse
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public int Code { get; set; }
    }
}
