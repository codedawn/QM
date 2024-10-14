using QM;

namespace Test
{
    public struct SessionIdleEvent : IEvent
    {
        public Session session;
        public long intervalTime;

        public SessionIdleEvent(Session session, long intervalTime)
        {
            this.session = session;
            this.intervalTime = intervalTime;
        }
    }
}
