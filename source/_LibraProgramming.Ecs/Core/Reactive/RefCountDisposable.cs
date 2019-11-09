using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RefCountDisposable : ICancelable
    {
        private readonly object gate;
        private IDisposable disposable;
        private bool isPrimaryDisposed;
        private int count;

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisposed => null == disposable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposable"></param>
        public RefCountDisposable(IDisposable disposable)
        {
            if (null == disposable)
            {
                throw new ArgumentNullException(nameof(disposable));
            }

            this.disposable = disposable;

            gate = new object();
            isPrimaryDisposed = false;
            count = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDisposable GetDisposable()
        {
            lock (gate)
            {
                if (null == disposable)
                {
                    return Disposable.Empty;
                }

                count++;

                return new InnerDisposable(this);
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            var token = default(IDisposable);

            lock (gate)
            {
                if (null != disposable)
                {
                    if (false == isPrimaryDisposed)
                    {
                        isPrimaryDisposed = true;

                        if (0 == count)
                        {
                            token = disposable;
                            disposable = null;
                        }
                    }
                }
            }

            if (null != token)
            {
                token.Dispose();
            }
        }

        private void Release()
        {
            var token = default(IDisposable);

            lock (gate)
            {
                if (null != disposable)
                {
                    count--;

                    if (isPrimaryDisposed)
                    {
                        if (0 == count)
                        {
                            token = disposable;
                            disposable = null;
                        }
                    }
                }
            }

            if (null != token)
            {
                token.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class InnerDisposable : IDisposable
        {
            private RefCountDisposable parent;
            private readonly object gate;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="parent"></param>
            public InnerDisposable(RefCountDisposable parent)
            {
                this.parent = parent;
                gate = new object();
            }

            /// <inheritdoc cref="IDisposable.Dispose" />
            public void Dispose()
            {
                RefCountDisposable owner;

                lock (gate)
                {
                    owner = parent;
                    parent = null;
                }

                if (null != owner)
                {
                    owner.Release();
                }
            }
        }
    }
}