using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Text;

namespace DotNettyRPC
{
    class ClientHandler : ChannelHandlerAdapter
    {
        private IClientWait _clientWait { get; }

        public ClientHandler(IClientWait clientWait)
        {
            _clientWait = clientWait;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuffer buffer = message as IByteBuffer;
            byte[] bytes = new byte[buffer.ReadableBytes];
            buffer.ReadBytes(bytes);
            _clientWait.Set(bytes);
        }
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}