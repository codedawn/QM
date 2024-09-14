

using DotNetty.Transport.Channels;
using System;
using System.Threading.Tasks;

namespace QM
{
    public class Connection : IConnection
    {
        private ILog _log = new NLogger(typeof(Connection));
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

        public bool IsConnect()
        {
            return (_channel.Open && _channel.Active && _channel.IsWritable);
        }

        public async Task Close()
        {
            await _channel.CloseAsync();
        }

        public async Task Send(IResponse response)
        {
            if (!IsConnect())
            {
                _log.Warn($"尝试给已经断开的连接发送消息Cid:{Cid}");
                return;
            }
            await _channel.WriteAndFlushAsync(response);
        }

        public async Task Send(IPush push)
        {
            if (!IsConnect())
            {
                _log.Warn($"尝试给已经断开的连接发送消息Cid:{Cid}");
                return;
            }
            await _channel.WriteAndFlushAsync(push);
        }
    }
}
