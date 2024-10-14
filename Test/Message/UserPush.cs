using MessagePack;
using QM;

namespace Test
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
