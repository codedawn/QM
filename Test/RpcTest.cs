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
            //DotNettyRPCTest();
            DotNettyRPCTest1();
            Console.ReadLine();
        }
        private class InnerMessageOpcode : IRpcMessageOpcode
        {
            public short GetIndex(Type type)
            {
                return MessageOpcode.Instance.GetIndex(type);
            }

            public Type GetType(short index)
            {
                return MessageOpcode.Instance.GetType(index);
            }
        }

        private async static void DotNettyRPCTest()
        {
            int threadCount = 1;
            int port = 39999;
            int count = 1;
            int errorCount = 0;
            MessageOpcodeHelper.SetMessageOpcode(new InnerMessageOpcode());
            RPCServer rPCServer = new RPCServer(port);
            rPCServer.RegisterService<IRemoteTest, RemoteTest>();
            rPCServer.Start();
            UserRequest user = new UserRequest() { Id = 582105291, Name = "fjeiw", Email = "25809219@gmai.com" };
            UserPush userPush = new UserPush();
            NetSession netSession = new NetSession();
            IRemoteTest remote = RPCClientFactory.GetClient<IRemoteTest, IResponse>("127.0.0.1", port);
            Stopwatch watch = new Stopwatch();
            List<Task> tasks = new List<Task>();
            watch.Start();
            await remote.PushTest(userPush, netSession);
            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    for (int j = 0; j < count; j++)
                    {
                        try
                        {
                            IResponse response = await remote.Test(user, netSession);
                            Console.WriteLine(response.ToString());
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

        private async static void DotNettyRPCTest1()
        {
            int threadCount = 1;
            int port = 40000;
            int count = 1000000;
            int errorCount = 0;
            MessageOpcodeHelper.SetMessageOpcode(new InnerMessageOpcode());
            RPCServer rPCServer = new RPCServer(port);
            rPCServer.RegisterService<IRemoteTest, RemoteTest>();
            rPCServer.Start();
            UserRequest user = new UserRequest() { Id = 582105291, Name = "fjeiw", Email = "25809219@gmai.com" };
            UserPush userPush = new UserPush();
            NetSession netSession = new NetSession();

            IRemoteTest remote = RPCClientFactory.GetClient<IRemoteTest, IResponse>("127.0.0.1", port);
            Stopwatch watch = new Stopwatch();
            List<Task> tasks = new List<Task>();
            watch.Start();
            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    Task<IResponse>[] tasks1 = new Task<IResponse>[count];
                    for (int j = 0; j < count; j++)
                    {
                        tasks1[j] = remote.Test(user, netSession);
                    }
                    IResponse[] responses = await Task.WhenAll(tasks1);
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
