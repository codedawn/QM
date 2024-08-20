using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class AsyncHelperTest
    {
        public static void Run()
        {
            Test1();
        }

        private static void Test1()
        {
            AsyncHelper.RunSync(async () =>
            {
                await F1();
                await F2();
            }
            );

            AsyncHelper.RunSync(async () => {
                await F1();
                return F2(); }
            );
            Console.WriteLine("Test1");
        }

        private async static Task F1()
        {
            await Task.Delay(2000);
            Console.WriteLine("F1");
        }

        private async static Task F2()
        {
            await Task.Delay(2000);
            Console.WriteLine("F2");
        }
    }
}
