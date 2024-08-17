namespace QM
{
    public class HeartBeatFilter : Filter
    {
        private long _idleTime = 5000;//ms

        public override bool Before(IMessage message, Session session)
        {
            if (message != null && message is Heatbeat heatbeat)
            {
                long last = session.lastAccessTime;
                long cur = TimeUtils.GetUnixTimestampMilliseconds();
                long interval = cur - last;
                session.lastAccessTime = cur;
                if (interval > _idleTime)
                {
                    EventSystem.Instance.Publish(new SessionIdleEvent(session, interval));
                }
                return false;
            }
            return true;
        }
    }
}
