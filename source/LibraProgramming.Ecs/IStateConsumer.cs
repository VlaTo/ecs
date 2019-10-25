namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public interface IStateConsumer<in TState>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void ApplyState(TState state);
    }
}