using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class ServerHandle : ChannelHandlerAdapter
    {
        private SocketServer socket;

        public ServerHandle(SocketServer socket)
        {
            this.socket = socket;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IMessage msg = message as IMessage;
            if(msg != null)
            {
                socket.OnMessage(msg, context.Channel);
            }
            base.ChannelRead(context, message);
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
           socket.Connect(context.Channel);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            socket.Disconnect(context.Channel);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            base.ExceptionCaught(context, exception);
            context.CloseAsync().Wait();
        }
    }
}
