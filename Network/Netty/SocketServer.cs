using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Network
{
    public class SocketServer : ISocket
    {
        private ServerBootstrap _bootstrap;
        private MultithreadEventLoopGroup bossGroup;
        private MultithreadEventLoopGroup workerGroup;
        private IChannel _bootstrapChannel;
        private int _port = 29999;
        public event Action<IMessage> onMessage;
        public event Action<IMessage> onConnect;
        public event Action<IMessage> onDisConnect;

        public void Start()
        {
            Run();
        }


        public void Connect()
        {
        }

        public void Disconnect()
        {
        }

        public void OnMessage(IMessage message)
        {
            Console.WriteLine(message.ToString());
        }

        public async void Run()
        {
            IProtocol protocol = new QMProtocol();
            bossGroup = new MultithreadEventLoopGroup();
            workerGroup = new MultithreadEventLoopGroup();
            _bootstrap = new ServerBootstrap();
            _bootstrap
                .Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;
                    pipeline.AddLast(new ProtocolEncode(protocol));
                    pipeline.AddLast(new ProtocolDecoder(protocol));
                    pipeline.AddLast(new ServerHandle(this));
                }));

            _bootstrapChannel = _bootstrap.BindAsync(_port).Result;
            Console.WriteLine("服务器启动成功");
            Console.ReadLine();
            await _bootstrapChannel?.CloseAsync();
        }

        public async void Close()
        {
            await _bootstrapChannel?.CloseAsync();
            Task.WaitAll(bossGroup?.ShutdownGracefullyAsync(), workerGroup?.ShutdownGracefullyAsync());
        }
    }
}
