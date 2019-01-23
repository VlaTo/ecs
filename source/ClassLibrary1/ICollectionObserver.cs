namespace ClassLibrary1
{
    public interface ICollectionObserver<in T> : ICompletable
    {
        void OnAdded(T item, int index);

        void OnRemoved(T item, int index);
    }
}