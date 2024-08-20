

using DotNetty.Transport.Channels;

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

        public void Send(IMessage message)
        {
            _channel.WriteAndFlushAsync(message);
        }

        public void Close()
        {
            Console.WriteLine("close");
            _channel.CloseAsync();
        }
    }
}
