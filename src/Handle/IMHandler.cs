namespace QM
{
    public interface IMHandler
    {
        public IResponse Handle(IMessage message, ISession session);
        public Type GetMessageType();
        public Type GetResponseType();
    }
}
