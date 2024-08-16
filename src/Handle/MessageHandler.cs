using QM.Network;
using src.Handle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Handle
{
    public abstract class MessageHandler<Request, Response> : IMHandler where Request : IRequest where Response : IResponse
    {   
        public Type GetMessageType()
        {
            return typeof(Request);
        }

        public Type GetResponseType()
        {
            return typeof(Response);
        }

        public IResponse Handle(IMessage message, Session session)
        {
            if (message is not Request request)
            {
                throw new ArgumentException("消息类型转换IRequest错误");
            }

            Response response = (Response)Activator.CreateInstance(GetResponseType());
            response.Id = request.Id;
            try
            {
                Run(request, response, session);
            }
            catch (Exception ex)
            {
                //todo error response
            }

            return response;
        }

        protected abstract void Run(Request request, Response response, Session session);
    }
}
