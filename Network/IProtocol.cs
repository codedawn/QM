using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QM.Network
{
    public interface IProtocol
    {
        public void Encode(object msg, IByteBuffer output);
        public void Decode(IByteBuffer input, List<object> output);
    }
}
