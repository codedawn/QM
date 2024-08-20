using QM;

namespace Test
{
    public class ApplicationTest
    {
        public static void Run()
        {
            Test2();
        }

        private static void Test1()
        {
            Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                SocketClient socketClient = new SocketClient();
                socketClient.RunClient();
            });

            Application application = Application.CreateApplication("Connector01", Application.Connector, 20000);
            application.Start();
            Console.ReadLine();
        }

        private static void Test2()
        {
            Application application = Application.CreateApplication("Server01", Application.Server, 30000);
            application.Start();
            Console.ReadLine();
        }
    }
}
