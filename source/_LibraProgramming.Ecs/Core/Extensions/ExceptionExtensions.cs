using System;

namespace LibraProgramming.Ecs.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static void Throw(this Exception exception)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}