using QM.Handle;
using QM.Network;
using src.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.Handle
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
