using System.Formats.Asn1;
using MessagePack;

namespace QM
{
    public interface IRequest : IMessage
    {
        public long Id { get; set; }
    }
}
