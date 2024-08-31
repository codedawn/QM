namespace QM
{
    [MessageHandler]
    public class UserMessageHandler : MessageHandler<User, UserResponse>
    {
        protected async override Task Run(User request, UserResponse response, ISession session)
        {
            RemoteSession remoteSession = session as RemoteSession;
            await EventSystem.Instance.Publish(new UserAddEvent() { Session = remoteSession });
            response.Name = request.Name;
            Console.WriteLine($"UserMessageHandler {request}");
        }
    }
}
