using MessagePack;

namespace QM
{
    [MessageIndex(2)]
    [MessagePackObject]
    public class UserResponse : IResponse 
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public int Code { get; set; }

        public override string ToString()
        {
            return $"{Id},{Name}";
        }
    }
}
