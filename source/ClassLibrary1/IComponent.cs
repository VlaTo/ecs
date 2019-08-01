namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public interface IComponent : IStateProvider<ComponentState>, ICloneable<IComponent>
    {
        /// <summary>
        /// 
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// 
        /// </summary>
        EntityBase Entity
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void Attach(EntityBase entity);

        /// <summary>
        /// 
        /// </summary>
        void Release();
    }
}