using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task InitializeAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}