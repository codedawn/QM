using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    [MessageIndex(310)]
    [MessagePackObject]
    public class UserAuthResponse : IResponse
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public int Code { get; set; }
    }
}
