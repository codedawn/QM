namespace QM
{
    [MessageHandler]
    public class UserMessageHandler : MessageHandler<User, UserResponse>
    {
        protected override void Run(User request, UserResponse response, ISession session)
        {
            RemoteSession remoteSession = session as RemoteSession;
            EventSystem.Instance.Publish(new UserAddEvent() { Session = remoteSession });
            response.Name = request.Name;
            Console.WriteLine($"UserMessageHandler {request}");
        }
    }
}
