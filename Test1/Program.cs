﻿namespace Test1
{
    public class ProgramTest
    {
        public static void Main(string[] args)
        {
            //Task.Run(async () =>
            //{
            //    await Task.Delay(2000);
            //    ClientTest.Run();
            //});
            //ClientTest.Run();
            // QMProtocolTest.Run();
            //int port = 30000;
            //if (args.Length > 0 )
            //{
            //    port = int.Parse( args[0] );
            //}
            ApplicationTest1.Run(30000);
            // RpcTest.Run();
            //ZookeeperServiceTest.Run();
            //EventSystemTest.Run();
            //Console.ReadLine();
            //ProxyTest.Run();
            Console.ReadLine();

        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e);
        }

        //public static async Task Main(string[] args)
        //{
        //    ProxyTest.Run();
        //}
    }
}
