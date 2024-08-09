using QM.Network;

namespace QM.Component
{
    public interface IChainFilter
    {
        public bool Before(Request request);
        public void After(Request request, Response response);
    }
}
