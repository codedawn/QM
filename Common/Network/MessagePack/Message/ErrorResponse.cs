using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    [MessageIndex(4)]
    [MessagePackObject]
    public class ErrorResponse : IResponse
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public int Code { get; set; }
        [Key(2)]
        public string Message { get; set; }
    }
}
