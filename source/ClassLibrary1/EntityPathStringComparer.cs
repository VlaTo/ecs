using System.Collections.Generic;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityPathStringComparer : IEqualityComparer<EntityPathString>
    {
        /// <summary>
        /// 
        /// </summary>
        public static EntityPathStringComparer Default { get; } = new EntityPathStringComparer();

        private EntityPathStringComparer()
        {
        }

        /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)" />
        public bool Equals(EntityPathString left, EntityPathString right)
        {
            if (ReferenceEquals(null, left))
            {
                return ReferenceEquals(null, right);
            }

            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(null, right))
            {
                return false;
            }

            return IsSegmentsEqual(left.Entry, right.Entry);
        }

        /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)" />
        public int GetHashCode(EntityPathString obj)
        {
            throw new System.NotImplementedException();
        }

        private static bool IsSegmentsEqual(EntityPathStringSegment left, EntityPathStringSegment right)
        {
            bool equal;

            while (true)
            {
                if (ReferenceEquals(null, left))
                {
                    equal = ReferenceEquals(null, right);
                    break;
                }

                if (ReferenceEquals(null, right))
                {
                    equal = false;
                    break;
                }

                equal = left.Equals(right);

                if (equal)
                {
                    left = left.Next;
                    right = right.Next;

                    continue;
                }

                break;
            }

            return equal;
        }
    }
}