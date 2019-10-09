using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IWorld
    {
        EntityBase Root { get; }

        void RegisterSystem<TSystem>() where TSystem : ISystem;

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}