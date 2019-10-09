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
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsEntityKey(this EntityPathSegment segment, out string key)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            if (segment is EntityKeySegment value)
            {
                key = value.Key;
                return true;
            }

            key = null;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static bool IsWildcard(this EntityPathSegment segment)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            return segment is EntityPathWildCardSegment;
        }
    }
}