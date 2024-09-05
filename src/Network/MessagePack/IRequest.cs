namespace QM
{
    public interface IRequest : IMessage
    {
        public long Id { get; set; }
    }
}
