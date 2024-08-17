using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class ClientHandler : ChannelHandlerAdapter
    {
        private int count;
        private int num = 1;
        private Stopwatch stopwatch = Stopwatch.StartNew();
        public override void ChannelActive(IChannelHandlerContext context)
        {
            User user = new User() { Id = 10214214, Name = "liu", Email = "wgjieo@gmail.com" };
            Heatbeat heatbeat = new Heatbeat();
            stopwatch.Start();
            for (int i = 0; i < num; i++)
            {
                context.Channel.WriteAndFlushAsync(user);
                context.Channel.WriteAndFlushAsync(heatbeat);
            }
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Console.WriteLine(message);
            if (++count == num)
            {
                stopwatch.Stop();
                Console.WriteLine($"处理{count}条消息花费{stopwatch.ElapsedMilliseconds}ms");
            }
            base.ChannelRead(context, message);
        }
    }
}
