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

        public IResponse Handle(IMessage message, ISession session)
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

        protected abstract void Run(Request request, Response response, ISession session);
    }
}
