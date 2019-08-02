namespace ClassLibrary1
{
    public interface ICollectionObserver<in T> : ICompletable
    {
        void OnAdded(T item);

        void OnRemoved(T item);
    }
}