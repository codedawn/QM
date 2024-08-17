namespace QM
{
    [MessageHandler]
    public class UserMessageHandler : MessageHandler<User, UserResponse>
    {
        protected override void Run(User request, UserResponse response, Session session)
        {
            response.Name = request.Name;
        }
    }
}
