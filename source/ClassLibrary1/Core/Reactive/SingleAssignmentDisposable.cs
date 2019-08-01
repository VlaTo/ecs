using System;

namespace ClassLibrary1.Core.Reactive
{
    public sealed class SingleAssignmentDisposable : ICancelable
    {
        private readonly object gate;
        private bool disposed;
        private IDisposable current;

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

        public SingleAssignmentDisposable()
        {
            gate = new object();
        }

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