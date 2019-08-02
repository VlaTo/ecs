using System;
using ClassLibrary1.Core.Path;
using ClassLibrary1.Core.Path.Segments;

namespace ClassLibrary1.Core.Extensions
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
    }
}