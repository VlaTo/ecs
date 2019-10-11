using LibraProgramming.Ecs.Core;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.Ecs.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class CancellationTokenExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task AsTask(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();

            using (cancellationToken.Register(() => tcs.SetComplete()))
            {
                await tcs.Task;
            }
        }
    }
}