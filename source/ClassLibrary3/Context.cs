namespace ClassLibrary3
{
    public abstract class Context
    {
        private readonly IState state;

        protected Context(IState state)
        {
            this.state = state;
        }
    }

    public abstract class Context<TState> where TState : IState
    {
        public TState State
        {
            get;
            set;
        }

        protected Context(TState state)
        {
            State = state;
        }
    }
}