using System;

namespace LibraProgramming.Ecs.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class Cancelable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposed"></param>
        /// <returns></returns>
        public static ICancelable Create(bool disposed)
        {
            return new BooleanDisposable(disposed);
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class BooleanDisposable : ICancelable
        {
            /// <summary>
            /// 
            /// </summary>
            public bool IsDisposed
            {
                get;
                private set;
            }

            /// <summary>
            /// 
            /// </summary>
            public BooleanDisposable()
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="isDisposed"></param>
            public BooleanDisposable(bool isDisposed)
            {
                IsDisposed = isDisposed;
            }

            /// <inheritdoc cref="IDisposable.Dispose" />
            public void Dispose()
            {
                if (false == IsDisposed)
                {
                    IsDisposed = true;
                }
            }
        }
    }
}