using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    /// <summary>
    /// 符号位（1bit）+ 时间戳（22bit）+ workerId（31bit）+ sequence（10bit）
    /// 用于允许短时间过期的序列号生成，workerId可以填充用户id
    /// </summary>
    public class SeqIdWorker
    {
        //序列号识位数
        private const int SequenceBits = 10;
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        private const int WorkerIdBits = 31;
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        private const int WorkerIdShift = SequenceBits;

        private long _workerId;
        private long _sequence;
        //上次生成ID的时间截
        private long _lastTimestamp = -1L;

        //时间毫秒左移32位
        private const int TimestampBits = 32 - SequenceBits;
        private const int TimestampLeftShift = WorkerIdBits + SequenceBits;
        private const long TimestampMask = -1L ^ (-1L << TimestampBits);

        public SeqIdWorker(long workerId)
        {
            // 如果超出范围就抛出异常
            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException(string.Format("worker Id 必须大于0，且不能大于MaxWorkerId： {0}", MaxWorkerId));
            }
            this._workerId = workerId;
        }

        private readonly object _lock = new Object();
        public long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();
                if (timestamp < _lastTimestamp)
                {
                    throw new Exception(string.Format("时间戳必须大于上一次生成ID的时间戳.  拒绝为{0}毫秒生成id", _lastTimestamp - timestamp));
                }

                //如果上次生成时间和当前时间相同,在同一毫秒内
                if (_lastTimestamp == timestamp)
                {
                    //sequence自增，和sequenceMask相与一下，去掉高位
                    _sequence = (_sequence + 1) & SequenceMask;
                    //判断是否溢出,也就是每毫秒内超过1024，当为1024时，与sequenceMask相与，sequence就等于0
                    if (_sequence == 0)
                    {
                        //等待到下一毫秒
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;
                return ((timestamp & TimestampMask) << TimestampLeftShift) | (_workerId << WorkerIdShift) | _sequence;
            }
        }

        // 防止产生的时间比之前的时间还要小（由于NTP回拨等问题）,保持增量的趋势.
        private long TilNextMillis(long lastTimestamp)
        {
            var timestamp = this.TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        // 获取当前的时间戳
        private long TimeGen()
        {
            return Time.GetUnixTimestampMilliseconds();
        }
    }
}
