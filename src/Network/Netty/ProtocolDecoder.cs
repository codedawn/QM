using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Network
{
    public class ProtocolDecoder : ByteToMessageDecoder
    {
        private readonly IProtocol protocol;

        public ProtocolDecoder(IProtocol protocol)
        {
            this.protocol = protocol;
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            protocol.Decode(input, output);
        }

    }
}
