using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class NLogTest
    {
        public static void Run()
        {
            Test1();
        }

        private static void Test1()
        {
            NLogger nLogger = new NLogger(typeof(NLogTest));
            nLogger.Debug("test debug");
            nLogger.Info("test info");
            nLogger.Warn("test warn");
            nLogger.Error("test error");
            try
            {
                Test2();
            }
            catch (Exception ex)
            {
                nLogger.Error(ex);
            }
        }

        private static void Test2()
        {
            throw new Exception("test2");
        }
    }
}
