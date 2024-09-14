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
            for (int i = 0; i < 1; i++)
            {
                Task.Run(() =>
                {
                    Test1(i);
                });
            }
            //Test1();
        }

        private async static void Test1(int id)
        {
            Client client = new Client();
            await client.ConnectAsync("127.0.0.1", 20000);
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = id; i < 2 + id; i++)
            {
                UserResponse userResponse = (UserResponse)await client.SendRequestAsync(new UserRequest() { Id = i, Name = "lirewi", Email = "rewr" });
            }
            stopwatch.Stop();
            Console.WriteLine($"{id}ClientTest 成功 {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
