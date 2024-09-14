using QM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class IdWorkerTest
    {
        public static void Run()
        {
            Test1();
            Test2();
        }

        private static void Test1()
        {
            IdWorker idWorker = new IdWorker(1, 1);
            Console.WriteLine(idWorker.NextId());
        }

        private static void Test2()
        {
            IdWorker idWorker = new IdWorker(1, 1);
            int count = 100;
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(idWorker.NextId());
                //idWorker.NextId();
            }
            stopwatch.Stop();
            Console.WriteLine($"生成{count}个id消耗{stopwatch.ElapsedMilliseconds}ms,平均每个{stopwatch.ElapsedMilliseconds/count}ms");
        }
    }
}
