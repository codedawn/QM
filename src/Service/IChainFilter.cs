using QM.Network;

namespace QM.Service
{
    public interface IChainFilter
    {
        public bool Before(IRequest request);
        public void After(IRequest request, IResponse response);
    }
}
