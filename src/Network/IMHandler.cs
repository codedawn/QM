namespace QM.Network
{
    public interface IMHandler
    {
        public void Handle(IRequest request);
        public Type GetRequestType();
        public Type GetResponseTye();
    }
}
