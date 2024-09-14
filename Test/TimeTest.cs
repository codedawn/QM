using QM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class TimeTest
    {
        public static void Run()
        {
            TestDate();
        }

        private static void TestDate()
        {
            Console.WriteLine(Time.ConvertUnixTimestampToDateTime(Time.GetUnixTimestampMilliseconds()));
            Console.WriteLine(Time.ConvertUnixTimestampToDateTime(Time.GetLocalUnixTimestampMilliseconds()));
            Debug.Assert(Time.GetUnixTimestampMilliseconds() / 1000 == Time.GetUnixTimestamp());
        }
    }
}
