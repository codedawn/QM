using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    public class program
    {
        public static void Main(string[] args)
        {
            //Application application = Application.CreateApplication("Room01", Application.Server, 9999);
            //application.AddComponent(new RoomComp());
            //application.Start();
            for (int i = 1; i <= 1; i++)
            {
                int tmp = i;
                Task.Run(() =>
                {
                    Test1(tmp * 1000000);
                });
            }
            Console.ReadLine();
        }

        private async static void Test1(int id)
        {
            Console.WriteLine(id);
            int count = 20;
            SocketClient client = new SocketClient();
            client.Init();
            await client.ConnectAsync("127.0.0.1", 20000);
            Stopwatch stopwatch = Stopwatch.StartNew();
            Task[] tasks = new Task[count];
            for (int i = 0; i < count; i++)
            {
                Task task = client.SendRequestAsync(new UserRequest() { Id = i + id, Name = "lirewi", Email = "rewr" });
                tasks[i] = task;
            }
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            Console.WriteLine($"{id}ClientTest 成功 平均：{stopwatch.ElapsedMilliseconds * 1.0/count}ms {client.GetDetail()}");
        }
    }
}
