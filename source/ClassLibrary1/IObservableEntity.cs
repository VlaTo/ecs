using System;

namespace ClassLibrary1
{
    public interface IObservableEntity
    {
        IDisposable Subscribe(IEntityObserver observer);
    }
}