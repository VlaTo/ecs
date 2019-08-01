using System;

namespace ClassLibrary1.Core.Extensions
{
    public static class EntityPathStringExtensions
    {
        public static bool IsRelative(this EntityPathString path)
        {
            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return false == (path.Entry is RootEntityPathStringSegment);
        }
    }
}