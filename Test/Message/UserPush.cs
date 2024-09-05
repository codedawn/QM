

using MessagePack;

namespace QM
{
    [MessageIndex(5)]
    [MessagePackObject]
    public class UserPush : IPush
    {
        [Key(0)]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
