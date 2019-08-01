using System;

namespace ClassLibrary1.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static void Throw(this Exception exception)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}