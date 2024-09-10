using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using QM;
using System;
using System.Text;

namespace DotNettyRPC
{
    class ClientHandler<T> : ChannelHandlerAdapter
    {
        private ILog _log = new NLogger(typeof(ClientHandler));
        private RPCResponseHandler<T> _responseHandler;
        public ClientHandler(RPCResponseHandler<T> responseHandler)
        {
            _responseHandler = responseHandler;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuffer buffer = message as IByteBuffer;
            if (buffer != null)
            {
                try
                {
                    byte[] bytes = new byte[buffer.ReadableBytes];
                    buffer.ReadBytes(bytes);
                    _responseHandler.Handle(bytes);
                }
                finally
                {
                    buffer.Release();
                }
            }
            else
            {
                context.FireChannelRead(message);
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _log.Error(exception);
            context.CloseAsync();
        }
    }
}