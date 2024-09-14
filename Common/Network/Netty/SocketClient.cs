using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Net;
using System.Threading.Tasks;

namespace QM
{
    public class SocketClient
    {
        private Bootstrap _bootstrap;
        private Action<IPush> _onPush;
        private Action _onDisConnect;
        private IChannel _channel;
        private int _timeout = 1000 * 60;
        private CommonAwait<IResponse> _messageAwait;
        private long _requestCount;
        private long _responseCount;

        public SocketClient()
        {
            _messageAwait = new CommonAwait<IResponse>(_timeout, new TaskTimer(), "SocketClientAwait");
        }

        public void Init()
        {
            IProtocol protocol = new QMProtocol();
            var group = new MultithreadEventLoopGroup();

            _bootstrap = new Bootstrap();
            _bootstrap
                .Group(group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Option(ChannelOption.Allocator, UnpooledByteBufferAllocator.Default)//不用这个会oom
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new ProtocolEncode(protocol));
                    pipeline.AddLast(new ProtocolDecoder(protocol));
                    pipeline.AddLast(new ClientHandler(new ClientMessageHandler(this)));
                }));
        }

        public async Task<IChannel> ConnectAsync(string ip, int port)
        {
            _channel = await _bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(ip), port));
            return _channel;
        }

        public async Task<IResponse> SendRequestAsync(IRequest request)
        {
            _requestCount++;
            Task<IResponse> task = _messageAwait.Start(request.Id);
            await _channel.WriteAndFlushAsync(request);
            return await task;
        }

        public async Task SendNotifyAsync(IMessage message)
        {
            await _channel.WriteAndFlushAsync(message);
        }

        public void OnPush(IPush push)
        {
            _onPush?.Invoke(push);
        }

        public void OnResponse(IResponse response)
        {
            _responseCount++;
            _messageAwait.Set(response.Id, response);
        }

        public void OnDisConnect()
        {
            _onDisConnect?.Invoke();
        }

        public void SetOnPushCallback(Action<IPush> action)
        {
            _onPush += action;
        }

        public void SetOnDisConnectCallback(Action action)
        {
            _onDisConnect += action;
        }

        public async Task CloseAsync()
        {
            await _channel.CloseAsync();
        }

        public string GetDetail()
        {
            return $"requestCount:{_requestCount}, responseCount:{_responseCount}";
        }
    }
}
