namespace QM.Network
{
    public interface IMHandler
    {
        public void Handle(Request request);
        public Type GetRequestType();
        public Type GetResponseTye();
    }
}
