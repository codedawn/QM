namespace QM
{
    /// <summary>
    /// 定义消息index
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageIndexAttribute : BaseAttribute
    {
        public short index;

        public MessageIndexAttribute(short index)
        {
            this.index = index;
        }
    }
}
