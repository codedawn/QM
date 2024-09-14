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
            // 获取当前的 UTC 时间
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            // 将时间转换到东八区 (UTC+08:00)
            DateTimeOffset easternTime = utcNow.ToOffset(TimeSpan.FromHours(8));

            // 返回该东八区时间的 Unix 时间戳，以毫秒为单位
            return easternTime.ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取时间戳（毫秒）
        /// </summary>
        /// <returns></returns>
        public static long GetUnixTimestampMilliseconds()
        {
            // 获取当前的 UTC 时间
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            // 将时间转换到东八区 (UTC+08:00)
            DateTimeOffset easternTime = utcNow.ToOffset(TimeSpan.FromHours(8));

            // 返回该东八区时间的 Unix 时间戳，以毫秒为单位
            return easternTime.ToUnixTimeMilliseconds();
        }

        public static string ConvertUnixTimestampToDateTime(long unixTimestampMilliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimestampMilliseconds).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static long GetLocalUnixTimestampMilliseconds()
        {
            // 获取当前系统的本地时间
            DateTimeOffset localNow = DateTimeOffset.Now;

            // 返回该本地时间的 Unix 时间戳，以毫秒为单位
            return localNow.ToUnixTimeMilliseconds();
        }
    }

}
