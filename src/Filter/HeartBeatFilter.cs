using System.Threading.Tasks;

namespace QM
{
    [Filter(includeServer = Application.Connector)]
    public class HeartBeatFilter : Filter
    {
        private long _idleTime = 5000;//ms

        public override async Task<bool> Before(IMessage message, ISession session)
        {
            Session se = (Session)session;
            if (message != null && message is Heatbeat heatbeat)
            {
                return false;
            }
            return true;
        }
    }
}
