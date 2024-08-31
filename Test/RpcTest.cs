using DotNettyRPC;
using QM;
using System.Diagnostics;

namespace Test
{
    public class RpcTest
    {
        public static void Run()
        {
            //Test1();
            DotNettyRPCTest();
            Console.ReadLine();
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

        private static async void DotNettyRPCTest()
        {
            int threadCount = 1;
            int port = 39999;
            int count = 100000;
            int errorCount = 0;
            MessageOpcodeHelper.SetMessageOpcode(new InnerMessageOpcode());
            RPCServer rPCServer = new RPCServer(port);
            rPCServer.RegisterService<IRemote, Remote>();
            rPCServer.Start();
            User user = new User() { Id = 582105291, Name = "fjeiw", Email = "25809219@gmai.com" };
            NetSession netSession = new NetSession();
            IRemote remote = RPCClientFactory.GetClient<IRemote, IResponse>("127.0.0.1", port);
            Stopwatch watch = new Stopwatch();
            List<Task> tasks = new List<Task>();
            watch.Start();
            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    for (int j = 0; j < count; j++)
                    {
                        try
                        {
                            IResponse response = await remote.Test(user, netSession);
                            //Console.WriteLine(response.ToString());
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            throw;
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            watch.Stop();
            Console.WriteLine($"并发数:{threadCount},运行:{count}次,每次耗时:{(double)watch.ElapsedMilliseconds / count}ms");
            Console.WriteLine($"错误次数：{errorCount}");
            Console.ReadLine();
        }
    }
}
