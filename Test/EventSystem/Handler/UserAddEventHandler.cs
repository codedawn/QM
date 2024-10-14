using QM;

namespace Test
{
    [EventHandler]
    public class UserAddEventHandler : QM.EventHandler<UserAddEvent>
    {
        public override async Task Run(UserAddEvent e)
        {
            UserPush userPush = new UserPush() { Name = "push" };
            await Application.current.GetComponent<RpcComp>().PushToConnector(userPush, e.Session.ServerId, e.Session.Sid);

            UserPush userPush1 = new UserPush() { Name = "broadcast" };
            await Application.current.GetComponent<RpcComp>().Broadcast(userPush1);
        }
    }
}
