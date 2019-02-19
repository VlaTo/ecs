using System;

namespace ClassLibrary1.Extensions
{
    internal static class EntityPathStringExtensions
    {
        public static bool IsRoot(this EntityPathStringSegment segment)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            return segment is RootEntityPathStringSegment;
        }
    }
}