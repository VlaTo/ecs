namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICompletable : IError
    {
        /// <summary>
        /// 
        /// </summary>
        void OnCompleted();
    }
}