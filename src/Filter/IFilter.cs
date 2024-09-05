using System.Threading.Tasks;

namespace QM
{
    public interface IFilter
    {
        public Task<bool> Before(IMessage message, ISession session);
        public Task After(IMessage message, IResponse response, ISession session);
    }
}
