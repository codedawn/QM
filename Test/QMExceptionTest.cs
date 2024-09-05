using QM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class QMExceptionTest
    {
        public static void Run()
        {
            Test1();
        }

        private static void Test1()
        {
            try
            {
                Test2();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                Test3();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void Test2()
        {
            throw new QMException(ErrorCode.AwaitNotFoundId, "test");
        }

        private static void Test3()
        {
            throw new Exception("test3");
        }
    }
}
