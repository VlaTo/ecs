using System;

namespace ClassLibrary1.Core
{
    internal static class Condition<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Predicate<T> True => _ => true;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Predicate<T> False => _ => false;
    }
}