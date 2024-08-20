using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class SocketClient
    {
        public void RunClient()
        {
            IProtocol protocol = new QMProtocol();
            var group = new MultithreadEventLoopGroup(1);

            Bootstrap bootstrap = new Bootstrap();
            bootstrap
                .Group(group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new ProtocolEncode(protocol));
                    pipeline.AddLast(new ProtocolDecoder(protocol));
                    pipeline.AddLast(new ClientHandler());
                }));

            IChannel channel = bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Application.current.port)).Result;
            //await channel.CloseAsync();
        }
    }
}
