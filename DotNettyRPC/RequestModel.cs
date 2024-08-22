using MessagePack;
using System;
using System.Collections.Generic;

namespace DotNettyRPC
{
    [MessagePackObject]
    public class RequestModel
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public string ServiceName { get; set; }
        [Key(2)]
        public string MethodName { get; set; }
        [Key(3)]
        public List<short> ParamterIndexs { get; set; }
        [Key(4)]
        public List<object> Paramters { get; set; }
    }
}
