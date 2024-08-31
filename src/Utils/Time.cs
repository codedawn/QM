using System;
using System.Diagnostics;
using System.Globalization;

namespace QM
{
    public class Time
    {
        public static double LastTime { get; internal set; }
        public static double AccTime { get; internal set; }
        public static double DeltaTime { get; internal set; }

        /// <summary>
        /// 获取时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public static long GetUnixTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取时间戳（毫秒）
        /// </summary>
        /// <returns></returns>
        public static long GetUnixTimestampMilliseconds()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

}
