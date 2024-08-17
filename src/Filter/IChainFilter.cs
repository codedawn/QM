namespace QM
{
    public interface IChainFilter
    {
        public bool Before(IMessage message, Session session);
        public void After(IMessage message, IResponse response, Session session);
    }
}
