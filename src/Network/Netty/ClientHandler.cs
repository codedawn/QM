using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Network
{
    public class ClientHandler : ChannelHandlerAdapter
    {
        public override void ChannelActive(IChannelHandlerContext context)
        {
            User user = new User() { Id = 10214214, Name = "liu", Email = "wgjieo@gmail.com" };
            context.Channel.WriteAndFlushAsync(user);
        }
    }
}
