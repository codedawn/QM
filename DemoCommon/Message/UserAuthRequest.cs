using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    [MessageIndex(300)]
    [MessagePackObject]
    [MessageDispatch(ServerType.Server)]
    public class UserAuthRequest : IRequest
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public string Token { get; set; }
    }
}
