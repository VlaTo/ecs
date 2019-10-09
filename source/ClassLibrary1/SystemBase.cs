using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public abstract class SystemBase : ISystem
    {
        protected SystemBase()
        {
        }

        public virtual Task InitializeAsync(IWorld world)
        {
            return Task.CompletedTask;
        }

        public abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}