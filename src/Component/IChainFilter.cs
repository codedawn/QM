using QM.Network;

namespace QM.Component
{
    public interface IChainFilter
    {
        public bool Before(IRequest request);
        public void After(IRequest request, IResponse response);
    }
}
