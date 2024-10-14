using MessagePack;
using QM;

namespace Test
{
    [MessageDispatch(ServerType.Server)]
    [MessageIndex(1)]
    [MessagePackObject]
    public class UserRequest : IRequest
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string Email { get; set; }

        public UserRequest()
        {
        }

        public UserRequest(long id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"{Id},{Name},{Email}";
        }
    }
}
