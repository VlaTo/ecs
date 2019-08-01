namespace ClassLibrary1
{
    public interface IComponentCreator
    {
        IComponent Create(ComponentState state);
    }
}