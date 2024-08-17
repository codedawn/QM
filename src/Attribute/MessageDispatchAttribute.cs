namespace QM
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageDispatchAttribute : BaseAttribute
    {
        public string server;

        public MessageDispatchAttribute(string server)
        {
            this.server = server;
        }
    }
}
