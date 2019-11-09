using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SystemBase : ISystem
    {
        /// <summary>
        /// 
        /// </summary>
        protected SystemBase()
        {
        }

        /// <inheritdoc cref="ISystem.InitializeAsync" />
        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ISystem.ExecuteAsync" />
        public abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}