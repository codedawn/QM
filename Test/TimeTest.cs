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
            Console.WriteLine(Time.ConvertUnixTimestampToDateTime(Time.GetUtc8TimestampMilliseconds()));
            Console.WriteLine(Time.ConvertUnixTimestampToDateTime(Time.GetLocalUnixTimestampMilliseconds()));
            Debug.Assert(Time.GetUtc8TimestampMilliseconds() / 1000 == Time.GetUtc8Timestamp());
        }
    }
}
