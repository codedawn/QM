using MessagePack;

namespace Coldairarrow.DotNettyRPC
{
    [MessagePackObject]
    public class ResponseModel
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public bool Success { get; set; }
        [Key(2)]
        public short DataIndex { get; set; }
        [Key(3)]
        public byte[] Data { get; set; }
        [Key(4)]
        public string Msg { get; set; }
    }
}
