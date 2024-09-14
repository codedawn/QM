using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    [EventHandler]
    public class UserAddEventHandler : EventHandler<UserAddEvent>
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
