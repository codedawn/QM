namespace QM
{
    [Filter(includeServer = Application.Connector)]
    public class HeartBeatFilter : Filter
    {
        private long _idleTime = 5000;//ms

        public override bool Before(IMessage message, ISession session)
        {
            Session se = (Session)session;
            if (message != null && message is Heatbeat heatbeat)
            {
                long last = se.lastAccessTime;
                long cur = TimeUtils.GetUnixTimestampMilliseconds();
                long interval = cur - last;
                se.lastAccessTime = cur;
                if (interval > _idleTime)
                {
                    EventSystem.Instance.Publish(new SessionIdleEvent(se, interval));
                }
                return false;
            }
            return true;
        }
    }
}
