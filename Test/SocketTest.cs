﻿using QM;

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
                client.Init();
            });

            SocketServer server = new SocketServer();
            server.Start();
        }
    }
}
