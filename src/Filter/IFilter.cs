namespace QM
{
    public interface IFilter
    {
        public bool Before(IMessage message, ISession session);
        public void After(IMessage message, IResponse response, ISession session);
    }
}
