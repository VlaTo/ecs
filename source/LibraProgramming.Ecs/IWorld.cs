using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWorld
    {
        /// <summary>
        /// 
        /// </summary>
        Entity Root
        {
            get;
        }

        IReadOnlyList<ISystem> Systems
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSystem"></typeparam>
        void RegisterSystem<TSystem>() where TSystem : ISystem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}