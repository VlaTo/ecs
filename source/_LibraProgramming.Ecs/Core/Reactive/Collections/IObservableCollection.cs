using System;

namespace LibraProgramming.Ecs.Core.Reactive.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObservableCollection<out T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        IDisposable Subscribe(ICollectionObserver<T> observer);
    }
}