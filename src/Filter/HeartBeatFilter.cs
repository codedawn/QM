using System.Threading.Tasks;

namespace QM
{
    [Filter(includeServer = Application.Connector)]
    public class HeartBeatFilter : Filter
    {
        private long _idleTime = 5000;//ms

        public override Task<bool> Before(IMessage message, ISession session)
        {
            if (message != null && message is Heatbeat heatbeat)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }
}
