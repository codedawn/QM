using QM.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class SocketTest
    {
        public static void Run()
        {
            Test1();
        }

        private static void Test1()
        {
            Task.Run(() =>
            {
                Task.Delay(3000).Wait();
                SocketClient client = new SocketClient();
                client.RunClient();
            });

            SocketServer server = new SocketServer();
            server.Start();
        }
    }
}
