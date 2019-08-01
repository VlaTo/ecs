namespace ClassLibrary1
{
    public interface IPrototypeResolver
    {
        void Initialize(EntityState state);

        EntityBase Resolve(EntityPathString path);
    }
}