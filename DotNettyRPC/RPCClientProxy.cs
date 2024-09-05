using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using QM;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DotNettyRPC
{
    public class RPCClientProxy<T> : DynamicObject
    {
        private int _timeout;
        private IPEndPoint _ipEndPoint;
        private string _serviceName;
        private Type _serviceType;

        private long _idCount;
        private Bootstrap _bootstrap { get; }
        private CommonAwait<T> _rpcAwait { get; }
        private IChannel client;

        public RPCClientProxy(string serverIp, int port, Type serviceType, int timeout)
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);
            _serviceType = serviceType;
            _serviceName = serviceType.Name;
            _timeout = timeout;
            _rpcAwait = new CommonAwait<T>(_timeout, new TaskTimer());
            _bootstrap = new Bootstrap()
                .Group(new MultithreadEventLoopGroup())
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast("framing-enc", new LengthFieldPrepender(8));
                    pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 8, 0, 8));

                    pipeline.AddLast(new ClientHandler<T>(new RPCResponseHandler<T>(_rpcAwait)));
                }));
        }

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
                        throw new QMException(ErrorCode.RPCConnectFail, "连接到RPC服务端失败");
                    }
                }

                if (client != null)
                {
                    long id = Interlocked.Increment(ref _idCount);
                    Task task = _rpcAwait.Start(id);
                    RPCRequest requestModel = new RPCRequest
                    {
                        Id = id,
                        ServiceName = _serviceName,
                        MethodName = binder.Name,
                        ParamterIndexs = MessageOpcodeHelper.GetParameterIndexs(args),
                        Paramters = args.ToList()
                    };
                    byte[] bytes = MessagePackUtil.Serialize(requestModel);
                    IByteBuffer sendBuffer = Unpooled.WrappedBuffer(bytes);
                    result = task;
                    client.WriteAndFlushAsync(sendBuffer);
                    return true;
                }
                else
                {
                    throw new QMException(ErrorCode.RPCConnectFail, "连接到RPC服务端失败");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
