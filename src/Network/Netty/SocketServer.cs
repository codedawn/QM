﻿using DotNetty.Codecs;
using DotNetty.Common.Utilities;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class SocketServer : ISocket
    {
        public readonly ConnectionManager connectionManager;
        private ServerBootstrap _bootstrap;
        private MultithreadEventLoopGroup _bossGroup;
        private MultithreadEventLoopGroup _workerGroup;
        private IChannel _bootstrapChannel;
        private AttributeKey<IConnection> _connectionKey = AttributeKey<IConnection>.NewInstance("Connection");

        public event Action<IMessage, IConnection> onMessage;
        public event Action<IConnection> onConnect;
        public event Action<IConnection> onDisConnect;

        public void Start()
        {
            RunServer();
        }

        public void Connect(IChannel channel)
        {
            IConnection connection = new Connection(channel, RemoteUtil.parseRemoteAddress(channel));
            channel.GetAttribute(_connectionKey).Set(connection);
            onConnect?.Invoke(connection);
        }

        public void Disconnect(IChannel channel)
        {
            IConnection connection = channel.GetAttribute(_connectionKey).Get();
            onDisConnect?.Invoke(connection);
        }

        public void OnMessage(IMessage message, IChannel channel)
        {
            IConnection connection = channel.GetAttribute(_connectionKey).Get();
            onMessage?.Invoke(message, connection);
        }

        public void RunServer()
        {
            IProtocol protocol = new QMProtocol();
            _bossGroup = new MultithreadEventLoopGroup();
            _workerGroup = new MultithreadEventLoopGroup();
#if DEBUG
            bool isSimpleDebug = Application.current.GetDebug();
            if (isSimpleDebug)
                _workerGroup = new MultithreadEventLoopGroup(1);
#endif

            _bootstrap = new ServerBootstrap();
            _bootstrap
                .Group(_bossGroup, _workerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, int.MaxValue)
                .Option(ChannelOption.TcpNodelay, true)
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;
                    pipeline.AddLast(new ProtocolEncode(protocol));
                    pipeline.AddLast(new ProtocolDecoder(protocol));
                    pipeline.AddLast(new ServerHandle(this));
                }));

            _bootstrapChannel = _bootstrap.BindAsync(Application.current.port).Result;
        }

        public async void Close()
        {
            await _bootstrapChannel?.CloseAsync();
            Task.WaitAll(_bossGroup?.ShutdownGracefullyAsync(), _workerGroup?.ShutdownGracefullyAsync());
        }
    }
}
