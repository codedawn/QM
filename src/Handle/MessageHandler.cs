using System;
using System.Threading.Tasks;

namespace QM
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

        public async Task<IResponse> Handle(IMessage message, ISession session)
        {
            if (!(message is Request request))
            {
                throw new ArgumentException("消息类型转换IRequest错误");
            }

            Response response = (Response)Activator.CreateInstance(GetResponseType());
            response.Id = request.Id;
            response.Code = (int)NetworkCode.Success;
            try
            {
                await Run(request, response, session);
            }
            catch (Exception ex)
            {
                //todo error response
            }

            return response;
        }

        protected abstract Task Run(Request request, Response response, ISession session);
    }
}
