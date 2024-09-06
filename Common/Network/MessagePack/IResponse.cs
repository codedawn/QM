using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface IResponse : IMessage
    {
        public long Id { get; set; }
        public int Code { get; set; }
    }
}
