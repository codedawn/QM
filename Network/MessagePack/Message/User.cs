using MessagePack;

namespace QM.Network
{
    [MessageIndex(0)]
    [MessagePackObject]
    public class User : IRequest
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string Email { get; set; }
    }
}
