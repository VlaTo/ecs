using System;
using System.Runtime.ExceptionServices;

namespace ClassLibrary2
{
    internal static class ExceptionHelper
    {
        internal static Exception PrepareForRethrow(Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
            return exception;
        }
    }
}