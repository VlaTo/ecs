using System;

namespace ClassLibrary1
{
    public interface IError
    {
        void OnError(Exception error);
    }
}