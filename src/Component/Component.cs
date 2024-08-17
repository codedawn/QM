namespace QM
{
    public abstract class Component : IComponent
    {
        protected ComponentState state;
        protected Component() { state = ComponentState.Init; }

        public virtual void Start()
        {
            state = ComponentState.Start;
        }
        public virtual void AfterStart()
        {
            state = ComponentState.AfterStart;
        }

        public virtual void Stop()
        {
            state = ComponentState.Stop;
        }
    }
}
