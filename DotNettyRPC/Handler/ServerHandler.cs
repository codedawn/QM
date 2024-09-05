using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;

namespace DotNettyRPC
{
    class ServerHandler : ChannelHandlerAdapter
    {
        public ServerHandler(RPCServer rPCServer)
        {
            _rpcServer = rPCServer;
        }
        RPCServer _rpcServer { get; }
        public override async void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuffer msg = message as IByteBuffer;
            byte[] bytes = new byte[msg.ReadableBytes];
            msg.ReadBytes(bytes);
            base.ChannelRead(context, bytes);

            RPCRequest requestModel = MessagePackUtil.Deserialize<RPCRequest>(bytes);
            RPCResponse response = await _rpcServer.GetResponse(requestModel);
            byte[] sendMsg = MessagePackUtil.Serialize(response);
            await context.WriteAndFlushAsync(Unpooled.WrappedBuffer(sendMsg));
        }
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            context.CloseAsync();
            throw exception;
        }
    }
}