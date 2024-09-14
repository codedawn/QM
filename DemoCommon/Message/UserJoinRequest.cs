using MessagePack;
using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    [MessageDispatch(ServerType.Server)]
    [MessageIndex(100)]
    [MessagePackObject]
    public class UserJoinRequest : IRequest
    {
        [Key(0)]
        public long Id { get; set; }

        [Key(1)]
        public long UserId { get; set; }
    }
}
