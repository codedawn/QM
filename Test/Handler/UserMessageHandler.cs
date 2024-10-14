using QM;
using System;
using System.Threading.Tasks;

namespace Test
{
    [MessageHandler]
    public class UserMessageHandler : MessageHandler<UserRequest, UserResponse>
    {
        protected async override Task Run(UserRequest request, UserResponse response, ISession session)
        {
            //RemoteSession remoteSession = session as RemoteSession;
            //await EventSystem.Instance.Publish(new UserAddEvent() { Session = remoteSession });
            response.Name = request.Name;
        }
    }
}
