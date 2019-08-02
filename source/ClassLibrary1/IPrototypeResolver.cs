using ClassLibrary1.Core.Path;

namespace ClassLibrary1
{
    public interface IPrototypeResolver
    {
        void Initialize(EntityState state);

        EntityBase Resolve(EntityPath path);
    }
}