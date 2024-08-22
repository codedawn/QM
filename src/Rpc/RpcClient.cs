﻿using DotNettyRPC;
using System.Diagnostics;
using System.Net;

namespace QM
{
    public class RpcClient
    {
        public RpcClient()
        {
            MessageOpcodeHelper.SetMessageOpcode(new RpcMessageOpcode());
        }

        public void Start(int port)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote>("127.0.0.1", port);
            User user = new User() { Id = 582105291, Name = "fjeiw", Email = "25809219@gmai.com" };
            NetSession netSession = new NetSession();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                //IResponse message = client.Forward(user, netSession);
            }
            stopwatch.Stop();
            Console.WriteLine($"执行了{stopwatch.ElapsedMilliseconds}ms");
            // Console.WriteLine("Forward:" + message.ToString());
        }

        public IResponse Forward(IMessage message, NetSession netSession, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote>(iPEndPoint.Address.ToString(), iPEndPoint.Port);
            //return client.Forward(message, netSession);
            return null;
        }

        public void Push(IMessage message, NetSession netSession, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote>(iPEndPoint.Address.ToString(), iPEndPoint.Port);
            client.Push(message, netSession);
        }
    }
}
