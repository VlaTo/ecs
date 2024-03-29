﻿using System;
using LibraProgramming.Ecs.Core.Path.Segments;

namespace LibraProgramming.Ecs.Core.Path.Extensions
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

            return segment is PathRoot;
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

            if (segment is EntityKey value)
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
        public static bool IsWildCard(this EntityPathSegment segment)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            return segment is EntityKeyWildCard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static bool IsAnyPathLevel(this EntityPathSegment segment)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            return segment is PathWildCard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static bool IsUpLevel(this EntityPathSegment segment)
        {
            if (null == segment)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            return segment is UpLevel;
        }
    }
}