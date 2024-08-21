namespace QM
{
    /// <summary>
    /// 标记消息分发到指定类型的服务器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageDispatchAttribute : BaseAttribute
    {
        public string serverType;

        public MessageDispatchAttribute(string serverType)
        {
            this.serverType = serverType;
        }
    }
}
