using QM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
    public class ClientTest
    {
        public static void Run()
        {
            for (int i = 0; i < 20; i++)
            {
                Task.Run(() =>
                {
                    Test1();
                });
            }
            //Test1();
        }

        private async static void Test1()
        {
            Client client = new Client();
            await client.ConnectAsync("127.0.0.1", 20000);
            Stopwatch stopwatch = Stopwatch.StartNew();
            int count = 1000;
            Task[] tasks = new Task[count];
            for (int i = 0; i < count; i++)
            {
                tasks[i] = client.SendRequestAsync(new UserJoinRequest() { Id = IdGenerator.NextId(), UserId = IdGenerator.NextId()});
            }
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            Console.WriteLine($"ClientTest 成功,耗时：{stopwatch.ElapsedMilliseconds}ms 平均{stopwatch.ElapsedMilliseconds/count}ms");
        }
    }
}
