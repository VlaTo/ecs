using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary1.Core.Reactive
{
    public sealed class CompositeDisposable : ICollection<IDisposable>, ICancelable
    {
        private const int ShrinkThreshold = 64;

        private readonly object gate;
        private List<IDisposable> disposables;
        private bool disposed;
        private int count;

        public int Count => count;

        public bool IsReadOnly => false;

        public bool IsDisposed => disposed;

        public CompositeDisposable()
        {
            gate = new object();
            disposables = new List<IDisposable>();
        }

        public CompositeDisposable(int capacity)
        {
            if (0 < capacity)
            {
                throw new ArgumentException("", nameof(capacity));
            }
            
            gate = new object();
            disposables = new List<IDisposable>(capacity);
        }

        public CompositeDisposable(params IDisposable[] disposables)
        {
            if (null == disposables)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            gate = new object();
            this.disposables = new List<IDisposable>(disposables);
            count = disposables.Length;
        }

        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            if (null == disposables)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            gate = new object();
            this.disposables = new List<IDisposable>(disposables);
            count = this.disposables.Count;
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            var res = new List<IDisposable>(count);

            lock (gate)
            {
                foreach (var disposable in disposables)
                {
                    if (null != disposable)
                    {
                        res.Add(disposable);
                    }
                }
            }

            return res.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IDisposable item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            bool shouldDispose;

            lock (gate)
            {
                shouldDispose = disposed;

                if (false == disposed)
                {
                    disposables.Add(item);
                    count++;
                }
            }

            if (shouldDispose)
            {
                item.Dispose();
            }
        }

        public void Clear()
        {
            IDisposable[] currentDisposables;

            lock (gate)
            {
                currentDisposables = disposables.ToArray();

                disposables.Clear();
                count = 0;
            }

            foreach (var disposable in currentDisposables)
            {
                if (null == disposable)
                {
                    continue;
                }

                disposable.Dispose();
            }
        }

        public bool Contains(IDisposable item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            lock (gate)
            {
                return disposables.Contains(item);
            }
        }

        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            if (null == array)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            lock (gate)
            {
                var disArray = new List<IDisposable>();

                foreach (var item in disposables)
                {
                    if (null != item)
                    {
                        disArray.Add(item);
                    }
                }

                Array.Copy(
                    disArray.ToArray(),
                    0,
                    array,
                    arrayIndex,
                    array.Length - arrayIndex
                );
            }
        }

        public bool Remove(IDisposable item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var shouldDispose = false;

            lock (gate)
            {
                if (false == disposed)
                {
                    //
                    // List<T> doesn't shrink the size of the underlying array but does collapse the array
                    // by copying the tail one position to the left of the removal index. We don't need
                    // index-based lookup but only ordering for sequential disposal. So, instead of spending
                    // cycles on the Array.Copy imposed by Remove, we use a null sentinel value. We also
                    // do manual Swiss cheese detection to shrink the list if there's a lot of holes in it.
                    //
                    var index = disposables.IndexOf(item);

                    if (index >= 0)
                    {
                        shouldDispose = true;
                        disposables[index] = null;
                        count--;

                        var capacity = disposables.Capacity / 2;

                        if (disposables.Capacity > ShrinkThreshold && count < capacity)
                        {
                            var old = disposables;

                            disposables = new List<IDisposable>(capacity);

                            foreach (var d in old)
                            {
                                if (d != null)
                                {
                                    disposables.Add(d);
                                }
                            }
                        }
                    }
                }
            }

            if (shouldDispose)
            {
                item.Dispose();
            }

            return shouldDispose;
        }

        public void Dispose()
        {
            var currentDisposables = default(IDisposable[]);

            lock (gate)
            {
                if (false == disposed)
                {
                    disposed = true;
                    currentDisposables = disposables.ToArray();
                    disposables.Clear();
                    count = 0;
                }
            }

            if (null != currentDisposables)
            {
                foreach (var disposable in currentDisposables)
                {
                    if (null != disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }
    }
}