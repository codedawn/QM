﻿using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading;

namespace DotNettyRPC
{
    public class RPCClientProxy<IResult> : DynamicObject
    {
        public RPCClientProxy(string serverIp, int port, Type serviceType, int timeout)
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);
            _serviceType = serviceType;
            _serviceName = serviceType.Name;
            _timeout = timeout;
            _clientWait = new ClientWait<IResult>(_timeout);
            _bootstrap = new Bootstrap()
                .Group(new MultithreadEventLoopGroup())
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast("framing-enc", new LengthFieldPrepender(8));
                    pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 8, 0, 8));

                    pipeline.AddLast(new ClientHandler(_clientWait));
                }));
        }
        private int _timeout;
        private IPEndPoint _ipEndPoint;
        private string _serviceName;
        private Type _serviceType;
        private Bootstrap _bootstrap { get; }
        private ClientWait<IResult> _clientWait { get; }
        private long _idCount;
        private IChannel client;
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                if (client == null || !(client.Open && client.Active && client.IsWritable))
                {
                    try
                    {
                        client = AsyncHelper.RunSync(() => _bootstrap.ConnectAsync(_ipEndPoint));
                    }
                    catch
                    {
                        throw new Exception("连接到RPC服务端失败");
                    }
                }

                if (client != null)
                {
                    long id = Interlocked.Increment(ref _idCount);
                    _clientWait.Start(id);
                    RequestModel requestModel = new RequestModel
                    {
                        Id = id,
                        ServiceName = _serviceName,
                        MethodName = binder.Name,
                        ParamterIndexs = MessageOpcodeHelper.GetParameterIndexs(args),
                        Paramters = args.ToList()
                    };
                    byte[] bytes = MessagePackUtil.Serialize(requestModel);
                    IByteBuffer sendBuffer = Unpooled.WrappedBuffer(bytes);
                    result = CallRpc(id);
                    client.WriteAndFlushAsync(sendBuffer);

                    return true;
                }
                else
                {
                    throw new Exception("连接到服务端失败!");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private object CallRpc(long id)
        {
            return _clientWait.Get(id);
        }
    }
}
