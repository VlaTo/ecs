using System;
using ClassLibrary1.Core.Path.Segments;

namespace ClassLibrary1.Core.Path.Extensions
{
    internal static class EntityPathSegmentExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static bool IsRoot(this EntityPathSegment segment)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            return segment is EntityPathRootSegment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="str"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="wildcard"></param>
        /// <returns></returns>
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