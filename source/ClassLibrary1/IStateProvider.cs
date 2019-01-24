namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public interface IStateProvider<out TState>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        TState GetState();
    }
}