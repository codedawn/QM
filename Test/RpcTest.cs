using Coldairarrow.DotNettyRPC;
using DotNettyRPC;
using DotNettyRPC.Helper;
using QM.Network;
using QM.Rpc;
using System.Diagnostics;

namespace Test
{
    public class RpcTest
    {
        public static void Run()
        {
            //Test1();
            DotNettyRPCTest();
        }
        private class InnerMessageOpcode : IRpcMessageOpcode
        {
            public short? GetIndex(Type type)
            {
                return MessageOpcode.Instance.GetIndex(type);
            }

            public Type GetType(short index)
            {
                return MessageOpcode.Instance.GetType(index);
            }
        }

        private static void Test1()
        {
           
            MessageOpcodeHelper.SetMessageOpcode(new InnerMessageOpcode());
            for (int i = 0; i < 10; i++)
            {
                Task.Run(() =>
                {
                    RpcClient client = new RpcClient();
                    client.Start(9999);
                });
            }

            RpcServer server = new RpcServer();
            server.Start();
        }

        private static void DotNettyRPCTest()
        {
            int threadCount = 1;
            int port = 39999;
            int count = 10000;
            int errorCount = 0;
            MessageOpcodeHelper.SetMessageOpcode(new InnerMessageOpcode());
            RPCServer rPCServer = new RPCServer(port);
            rPCServer.RegisterService<IRemote, Remote>();
            rPCServer.Start();
            IRemote client = null;
            client = RPCClientFactory.GetClient<IRemote>("127.0.0.1", port);
            User user = new User() { Id = 582105291, Name = "fjeiw", Email = "25809219@gmai.com" };
            NetSession netSession = new NetSession();
            client.Forward(user, netSession);
            Stopwatch watch = new Stopwatch();
            List<Task> tasks = new List<Task>();
            watch.Start();
            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < count; j++)
                    {
                        //string msg = string.Empty;
                        try
                        {
                            client.Forward(user, netSession);
                            //msg = client.SayHello("Hello");
                            //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}:{msg}");
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            //Console.WriteLine(ExceptionHelper.GetExceptionAllMsg(ex));
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            watch.Stop();
            Console.WriteLine($"并发数:{threadCount},运行:{count}次,每次耗时:{(double)watch.ElapsedMilliseconds / count}ms");
            Console.WriteLine($"错误次数：{errorCount}");
        }
    }
}
