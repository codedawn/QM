namespace QM
{
    public interface IMHandler
    {
        public IResponse Handle(IMessage message, Session session);
        public Type GetMessageType();
        public Type GetResponseType();
    }
}
