using System;
using System.Diagnostics;
using System.Globalization;

namespace QM
{
    public class Time
    {
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
