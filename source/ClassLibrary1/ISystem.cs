using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface ISystem
    {
        Task InitializeAsync(IWorld world);

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}