namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityObserver : ICompletable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        void OnAdded(IComponent component);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        void OnRemoved(IComponent component);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public interface IEntityObserver<in TComponent> : ICompletable
        where TComponent : IComponent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        void OnAdded(TComponent component);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        void OnRemoved(TComponent component);
    }
}