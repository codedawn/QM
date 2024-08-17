namespace QM
{
    public class Filter : IChainFilter
    {
        public virtual void After(IMessage request, IResponse response, Session session)
        {
        }

        public virtual bool Before(IMessage request, Session session)
        {
            return true;
        }
    }
}
