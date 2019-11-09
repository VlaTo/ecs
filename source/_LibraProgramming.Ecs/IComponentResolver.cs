namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IComponentResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        IComponent Resolve(string alias);
    }
}