using System;

namespace ClassLibrary1
{
    public class EmptyEntityObserver : IEntityObserver
    {
        public static readonly EmptyEntityObserver Instance;

        private EmptyEntityObserver()
        {
        }

        static EmptyEntityObserver()
        {
            Instance = new EmptyEntityObserver();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnAdded(IComponent component)
        {
        }

        public void OnRemoved(IComponent component)
        {
        }
    }
}