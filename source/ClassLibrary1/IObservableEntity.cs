using System;

namespace ClassLibrary1
{
    public interface IObservableEntity
    {
        IDisposable Subscribe(IEntityObserver observer);
    }

    public interface IObservableEntity<out TComponent>
        where TComponent : IComponent
    {
        IDisposable Subscribe(IEntityObserver<TComponent> observer);
    }
}