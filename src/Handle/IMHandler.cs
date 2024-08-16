using QM.Network;

namespace QM.Handle
{
    public interface IMHandler
    {
        public IResponse Handle(IMessage message, Session session);
        public Type GetMessageType();
        public Type GetResponseType();
    }
}
