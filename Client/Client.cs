﻿using System.Threading.Tasks;

namespace QM
{
    public class Client
    {
        private SocketClient _socketClient;

        public Client()
        {
            _socketClient = new SocketClient();
            _socketClient.Init();
        }

        public async Task ConnectAsync(string ip, int port)
        {
            await _socketClient.ConnectAsync(ip, port);
        }

        public async Task<IResponse> SendRequestAsync(IRequest request)
        {
            return await _socketClient.SendRequestAsync(request);
        }

        public async Task SendNotifyAsync(IMessage message)
        {
            await _socketClient.SendNotifyAsync(message);
        }
    }
}
