using System;

namespace QM
{
    /// <summary>
    /// 定义消息index，用于编码
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
