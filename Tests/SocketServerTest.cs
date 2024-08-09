using QM.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Tests
{
    public class SocketServerTest
    {
        public void Run()
        {
            Test1();
        }

        private void Test1()
        {
            Task.Run(() => {
                Task.Delay(5000).Wait();
                SocketClient client = new SocketClient();
                client.RunClient();
            });
            SocketServer socketServer = new SocketServer();
            socketServer.Start();
        }
    }
}
