using System;

namespace ClassLibrary1.Core
{
    public interface ICancelable : IDisposable
    {
        bool IsDisposed
        {
            get;
        }
    }
}