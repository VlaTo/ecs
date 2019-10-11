namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IComponent : ICloneable<IComponent>
    {
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