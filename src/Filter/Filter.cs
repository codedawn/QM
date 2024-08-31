namespace QM
{
    public class Filter : IFilter
    {
        public virtual Task After(IMessage request, IResponse response, ISession session)
        {
            return Task.CompletedTask;
        }
        

        public virtual Task<bool> Before(IMessage request, ISession session)
        {
            return Task.FromResult(true);
        }
    }
}
