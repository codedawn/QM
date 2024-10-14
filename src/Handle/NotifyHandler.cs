using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public abstract class NotifyHandler<NotifyMessage> : IMHandler
    {
        private ILog _Log = new NLogger(typeof(NotifyHandler<NotifyMessage>));
        public Type GetMessageType()
        {
            return typeof(NotifyMessage);
        }

        public Type GetResponseType()
        {
            return null;
        }

        public async Task<IResponse> Handle(IMessage message, ISession session)
        {
            try
            {
                await Run((NotifyMessage)message, session);
            }
            catch (Exception e)
            {
                _Log.Error(e);
            }
           
            return null;
        }

        public abstract Task Run(NotifyMessage notify, ISession session);

    }
}
