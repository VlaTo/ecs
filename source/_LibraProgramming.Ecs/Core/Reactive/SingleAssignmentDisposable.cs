using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SingleAssignmentDisposable : ICancelable
    {
        private readonly object gate;
        private bool disposed;
        private IDisposable current;

        /// <inheritdoc cref="ICancelable.IsDisposed" />
        public bool IsDisposed
        {
            get
            {
                lock (gate)
                {
                    return disposed;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDisposable Disposable
        {
            get => current;
            set
            {
                IDisposable old;
                bool alreadyDisposed;

                lock (gate)
                {
                    alreadyDisposed = disposed;
                    old = current;

                    if (false == alreadyDisposed)
                    {
                        if (null == value)
                        {
                            return;
                        }

                        current = value;
                    }
                }

                if (alreadyDisposed && null != value)
                {
                    value.Dispose();
                    return;
                }

                if (null != old)
                {
                    throw new InvalidOperationException("Disposable is already set");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SingleAssignmentDisposable()
        {
            gate = new object();
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            IDisposable old = null;

            lock (gate)
            {
                if (false == disposed)
                {
                    disposed = true;
                    old = current;
                    current = null;
                }
            }

            if (null != old)
            {
                old.Dispose();
            }
        }
    }
}