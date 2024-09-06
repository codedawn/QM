

using DotNetty.Transport.Channels;
using System;
using System.Threading.Tasks;

namespace QM
{
    public class Connection : IConnection
    {
        private readonly IChannel _channel;
        private readonly string _address;
        public string Address { get { return _address; } }

        private string _cid;
        public string Cid { get { return _cid; } set { _cid = value; } }

        public Connection(IChannel channel, string address)
        {
            this._channel = channel;
            this._address = address;
            this._cid = address;
        }

        public Connection(IChannel channel, string address, string cid)
        {
            this._channel = channel;
            this._address = address;
            this._cid = cid;
        }

        public async Task Send(IMessage message)
        {
            if (!IsConnect()) return;
            await _channel.WriteAndFlushAsync(message);
        }

        public bool IsConnect()
        {
            return (_channel.Open && _channel.Active && _channel.IsWritable);
        }

        public async Task Close()
        {
            await _channel.CloseAsync();
        }
    }
}
