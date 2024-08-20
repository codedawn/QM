namespace QM
{
    public class Filter : IFilter
    {
        public virtual void After(IMessage request, IResponse response, ISession session)
        {
        }

        public virtual bool Before(IMessage request, ISession session)
        {
            return true;
        }
    }
}
