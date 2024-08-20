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
        public override void Run(UserAddEvent e)
        {
            UserPush userPush = new UserPush() { Name = "useradd" };
            Application.current.GetComponent<RpcComp>().Push(userPush, e.Session.serverId, e.Session.Sid);
        }
    }
}
