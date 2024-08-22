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
            RequestModel requestModel = MessagePackUtil.Deserialize<RequestModel>(bytes);
            ResponseModel response = await _rpcServer.GetResponse(requestModel);
            byte[] sendMsg = MessagePackUtil.Serialize(response);
            context.WriteAndFlushAsync(Unpooled.WrappedBuffer(sendMsg));

            //context.CloseAsync();
        }
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}