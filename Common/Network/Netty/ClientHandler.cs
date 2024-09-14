using DotNetty.Transport.Channels;
using System;

namespace QM
{
    public class ClientHandler : ChannelHandlerAdapter
    {
        private ILog _log = new NLogger(typeof(ClientHandler));
        private ClientMessageHandler _handler;

        public ClientHandler(ClientMessageHandler handler)
        {
            _handler = handler;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IMessage msg = message as IMessage;
            if (msg != null)
            {
                _handler.Handle(msg);
            }
            base.ChannelRead(context, message);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            base.ChannelUnregistered(context);
            _handler.OnDisConnect();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _log.Error(exception);
            context.Channel.CloseAsync();
        }
    }
}
