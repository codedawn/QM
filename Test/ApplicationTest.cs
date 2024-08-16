using QM;
using QM.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
