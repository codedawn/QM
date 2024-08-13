using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Network
{
    public class ProtocolEncode : MessageToByteEncoder<IMessage>
    {
        private readonly IProtocol protocol;

        public ProtocolEncode(IProtocol protocol)
        {
            this.protocol = protocol;
        }

        protected override void Encode(IChannelHandlerContext context, IMessage message, IByteBuffer output)
        {
            protocol.Encode(message, output);
        }
    }
}
