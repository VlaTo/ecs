namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public interface IStateAcceptor<in TState>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void SetState(TState state);
    }
}