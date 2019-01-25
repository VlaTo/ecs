using System.Collections.Generic;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityPathStringSegmentComparer : IEqualityComparer<EntityPathStringSegment>
    {
        /// <summary>
        /// 
        /// </summary>
        public static EntityPathStringSegmentComparer Invariant { get; } = new EntityPathStringSegmentComparer();

        private EntityPathStringSegmentComparer()
        {
        }

        /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)" />
        public bool Equals(EntityPathStringSegment x, EntityPathStringSegment y)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)" />
        public int GetHashCode(EntityPathStringSegment obj)
        {
            throw new System.NotImplementedException();
        }
    }
}