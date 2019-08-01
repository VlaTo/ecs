using System;

namespace ClassLibrary1
{
    public sealed class DisposedEntityObserver : IEntityObserver
    {
        public static readonly DisposedEntityObserver Instance;

        private DisposedEntityObserver()
        {
        }

        static DisposedEntityObserver()
        {
            Instance = new DisposedEntityObserver();
        }

        public void OnCompleted()
        {
            throw new ObjectDisposedException(String.Empty);
        }

        public void OnError(Exception error)
        {
            throw new ObjectDisposedException(String.Empty);
        }

        public void OnAdded(IComponent component)
        {
            throw new ObjectDisposedException(String.Empty);
        }

        public void OnRemoved(IComponent component)
        {
            throw new ObjectDisposedException(String.Empty);
        }
    }
}