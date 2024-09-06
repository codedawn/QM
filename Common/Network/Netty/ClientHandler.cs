using DotNetty.Transport.Channels;

namespace QM
{
    public class ClientHandler : ChannelHandlerAdapter
    {
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
    }
}
