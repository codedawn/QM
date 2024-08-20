using QM;

namespace Test
{
    public class ZookeeperServiceTest
    {
        public static void Run()
        {
            Test1();
        }

        private static void Test1()
        {
            ZookeeperService zookeeperService = new ZookeeperService();
            AsyncHelper.RunSync(() => zookeeperService.StartAsync());
            AsyncHelper.RunSync(() => zookeeperService.RegisterAsync("/server:server1" + "127.0.0.1:29999", "127.0.0.1:29999"));
           // List<string> servers = AsyncHelper.RunSync(() => zookeeperService.GetServersAsync());

            Console.ReadLine();
            zookeeperService.Stop();
        }
    }
}
