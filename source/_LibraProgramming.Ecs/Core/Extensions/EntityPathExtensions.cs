using System;
using LibraProgramming.Ecs.Core.Path.Segments;
using EntityPath = LibraProgramming.Ecs.Core.Path.EntityPath;

namespace LibraProgramming.Ecs.Core.Extensions
{
    public static class EntityPathExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsRelative(this EntityPath path)
        {
            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return false == (path.Entry is EntityPathRootSegment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsAbsolute(this EntityPath path)
        {
            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return path.Entry is EntityPathRootSegment;
        }
    }
}