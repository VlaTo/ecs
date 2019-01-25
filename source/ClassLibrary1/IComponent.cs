namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public interface IComponent : IStateProvider<ComponentState>, IStateAcceptor<ComponentState>, ICloneable<IComponent>
    {
        /// <summary>
        /// 
        /// </summary>
        string Alias { get; }

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