namespace ClassLibrary1.Core.Reactive.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICollectionObserver<in T> : ICompletable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void OnAdded(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void OnRemoved(T item);
    }
}