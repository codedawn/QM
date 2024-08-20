namespace QM
{
    public abstract class Component : IComponent
    {
        private ILog _log = new ConsoleLog();
        protected ComponentState state;
        protected Component() { state = ComponentState.Init; }

        public virtual void Start()
        {
            _log.Info($"{this} Start");
            state = ComponentState.Start;
        }
        public virtual void AfterStart()
        {
            _log.Info($"{this} AfterStart");
            state = ComponentState.AfterStart;
        }

        public virtual void Stop()
        {
            _log.Info($"{this} Stop");
            state = ComponentState.Stop;
        }
    }
}
