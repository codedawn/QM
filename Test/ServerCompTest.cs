using QM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ServerCompTest
    {
        public static void Run()
        {
            Test1();
        }

        private async static void Test1()
        {
            Application application = Application.CreateApplication("TestServerComp", Application.Server, 9933);
            Task.Run(() => { application.Start(); });
            ServerComp serverComp = application.GetComponent<ServerComp>();
            UserRequest request = new UserRequest() { Id = 482159821095, Name = "TestServerComp" };
            RemoteConnection remoteConnection = new RemoteConnection();
            RemoteSession session = new RemoteSession("TestServerComp", remoteConnection, "");
            int count = 1000000;
            Task[] tasks = new Task[count];
            await Task.Delay(1000);
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                tasks[i] = serverComp.GlobalHandleAsync(request, session);
            }
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            Console.WriteLine($"GlobalHandleAsync  {count}次执行消耗{stopwatch.ElapsedMilliseconds}ms");
        }
            
    }
}
