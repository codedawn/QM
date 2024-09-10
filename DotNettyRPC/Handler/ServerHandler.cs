using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using QM;
using System;

namespace DotNettyRPC
{
    class ServerHandler : ChannelHandlerAdapter
    {
        private ILog _log = new NLogger(typeof(ServerHandler));
        public ServerHandler(RPCServer rPCServer)
        {
            _rpcServer = rPCServer;
        }
        RPCServer _rpcServer { get; }
        public override async void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuffer buffer = message as IByteBuffer;
            if (buffer != null)
            {
                try
                {
                    byte[] bytes = new byte[buffer.ReadableBytes];
                    buffer.ReadBytes(bytes);
                    RPCRequest requestModel = MessagePackUtil.Deserialize<RPCRequest>(bytes);
                    RPCResponse response = await _rpcServer.GetResponse(requestModel);
                    byte[] sendMsg = MessagePackUtil.Serialize(response);
                    await context.WriteAndFlushAsync(Unpooled.WrappedBuffer(sendMsg));
                }
                finally
                {
                    buffer.Release();
                }
            }
            else
            {
                context.FireChannelRead(message);
            }
        }
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _log.Error(exception);
            context.CloseAsync();
        }
    }
}