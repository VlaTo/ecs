namespace ClassLibrary1.Core
{
    public interface IEntitySubject<TComponent> : IObservableEntity<TComponent>, IEntityObserver<TComponent>
        where TComponent : IComponent
    {
    }
}