namespace ClassLibrary1
{
    public interface IComponent
    {
        Entity Entity
        {
            get;
        }

        void Attach(Entity entity);

        void Release();
    }
}