using QM;

namespace Test
{
    public class ApplicationTest
    {
        public static void Run()
        {
            Test1();
        }

        private static void Test1()
        {
            Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                SocketClient socketClient = new SocketClient();
                socketClient.RunClient();
            });

            Application application = Application.CreatApplication();
            application.Start();
            Console.ReadLine();
        }
    }
}
