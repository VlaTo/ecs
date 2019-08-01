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

        public static bool IsString(this EntityPathStringSegment segment, out StringEntityPathStringSegment str)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            if (segment is StringEntityPathStringSegment value)
            {
                str = value;
                return true;
            }

            str = null;

            return false;
        }

        public static bool IsWildcard(this EntityPathStringSegment segment, out WildCardEntityPathStringSegment wildcard)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            if (segment is WildCardEntityPathStringSegment value)
            {
                wildcard = value;
                return true;
            }

            wildcard = null;

            return false;
        }
    }
}