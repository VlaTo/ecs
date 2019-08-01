namespace ClassLibrary1.Core
{
    public static class Cancelable
    {
        public static ICancelable Create(bool disposed)
        {
            return new BooleanDisposable(disposed);
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class BooleanDisposable : ICancelable
        {
            public bool IsDisposed
            {
                get;
                private set;
            }

            public BooleanDisposable()
            {
            }

            public BooleanDisposable(bool isDisposed)
            {
                IsDisposed = isDisposed;
            }

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