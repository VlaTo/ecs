using System.Collections.Generic;
using System.Reflection;

namespace ClassLibrary3
{
    internal static class Methods
    {
        public static class String
        {
            public static readonly MethodInfo Concat;

            static String()
            {
                var type = typeof(string);
                //Concat = type.GetMethod(nameof(System.String.Concat), new[] {typeof(object), typeof(object)});
                Concat = type.GetMethod(nameof(System.String.Concat), new[] {typeof(IEnumerable<string>)});
            }
        }
    }
}