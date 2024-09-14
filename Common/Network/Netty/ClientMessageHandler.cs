using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QM
{
    public class ClientMessageHandler
    {
        private SocketClient _socketClient;

        public ClientMessageHandler(SocketClient socketClient)
        {
            _socketClient = socketClient;
        }

        public void Handle(IMessage message)
        {
            if (message is IResponse response)
            {
                _socketClient.OnResponse(response);
            }
            else if (message is IPush push)
            {
                _socketClient.OnPush(push);
            }
            else
            {
                throw new QMException(ErrorCode.MessageInvalid, "收到无效消息");
            }
        }

        public void OnDisConnect()
        {
            _socketClient.OnDisConnect();
        }
    }
}
