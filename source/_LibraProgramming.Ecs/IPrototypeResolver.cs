using EntityPath = LibraProgramming.Ecs.Core.Path.EntityPath;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPrototypeResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void Initialize(EntityState state);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        EntityBase Resolve(EntityPath path);
    }
}