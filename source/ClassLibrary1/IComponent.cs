namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public interface IComponent : IStateProvider<ComponentState>
    {
        /// <summary>
        /// 
        /// </summary>
        Entity Entity
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void Attach(Entity entity);

        /// <summary>
        /// 
        /// </summary>
        void Release();
    }
}