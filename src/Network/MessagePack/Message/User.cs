using MessagePack;

namespace QM
{
    [MessageDispatch(Application.Server)]
    [MessageIndex(1)]
    [MessagePackObject]
    public class User : IRequest
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string Email { get; set; }

        public User()
        {
        }

        public User(long id, string name, string email)
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
