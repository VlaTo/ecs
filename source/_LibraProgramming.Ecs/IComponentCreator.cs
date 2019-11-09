namespace LibraProgramming.Ecs
{
    public interface IComponentCreator
    {
        IComponent Create(ComponentState state);
    }
}