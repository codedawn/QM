namespace QM
{
    public interface IMHandler
    {
        public Task<IResponse> Handle(IMessage message, ISession session);
        public Type GetMessageType();
        public Type GetResponseType();
    }
}
