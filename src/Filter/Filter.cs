using System.Threading.Tasks;

namespace QM
{
    public class Filter : IFilter
    {
        public virtual Task After(IMessage message, IResponse response, ISession session)
        {
            return Task.CompletedTask;
        }
        

        public virtual Task<bool> Before(IMessage message, ISession session)
        {
            return Task.FromResult(true);
        }
    }
}
