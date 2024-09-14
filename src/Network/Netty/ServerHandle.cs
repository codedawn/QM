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
        private ILog _log = new NLogger(typeof(ServerHandle));
        private SocketServer _socket;

        public ServerHandle(SocketServer socket)
        {
            this._socket = socket;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IMessage msg = message as IMessage;
            if(msg != null)
            {
                _socket.OnMessage(msg, context.Channel);
            }
            base.ChannelRead(context, message);
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
           _socket.Connect(context.Channel);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            _socket.Disconnect(context.Channel);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _log.Error(exception);
            context.CloseAsync();
        }
    }
}
