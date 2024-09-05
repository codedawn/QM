using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Text;

namespace DotNettyRPC
{
    class ClientHandler<T> : ChannelHandlerAdapter
    {
        private RPCResponseHandler<T> _responseHandler;
        public ClientHandler(RPCResponseHandler<T> responseHandler)
        {
            _responseHandler = responseHandler;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuffer buffer = message as IByteBuffer;
            byte[] bytes = new byte[buffer.ReadableBytes];
            buffer.ReadBytes(bytes);
            _responseHandler.Handle(bytes);
            base.ChannelRead(context, message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            //Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
            throw exception;
        }
    }
}