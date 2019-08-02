using System;
using ClassLibrary1.Core.Path;
using ClassLibrary1.Core.Path.Segments;

namespace ClassLibrary1.Extensions
{
    internal static class EntityPathStringExtensions
    {
        public static bool IsRoot(this EntityPathSegment segment)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            return segment is EntityPathRootSegment;
        }

        public static bool IsString(this EntityPathSegment segment, out EntityPathStringSegment str)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            if (segment is EntityPathStringSegment value)
            {
                str = value;
                return true;
            }

            str = null;

            return false;
        }

        public static bool IsWildcard(this EntityPathSegment segment, out EntityPathWildCardSegment wildcard)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            if (segment is EntityPathWildCardSegment value)
            {
                wildcard = value;
                return true;
            }

            wildcard = null;

            return false;
        }
    }
}