using System;
using System.Globalization;

namespace QM.Utils
{
    public static class TimeUtils
    {
        // 获取时间戳（秒）
        public static long GetUnixTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        // 获取时间戳（毫秒）
        public static long GetUnixTimestampMilliseconds()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

}
