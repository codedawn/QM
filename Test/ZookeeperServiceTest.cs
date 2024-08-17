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
            AsyncHelper.RunSync(async ()  =>
            {
                await zookeeperService.StartAsync();
                return zookeeperService.RegisterAsync("/server-" + "127.0.0.1:29999", "127.0.0.1:29999");
            });
           
            Console.ReadLine();
            zookeeperService.Stop();
        }
    }
}
