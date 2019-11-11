using System;

namespace LibraProgramming.Ecs.Core.Path.Segments
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class EntityPathSegment : IEquatable<EntityPathSegment>
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityPathSegment Next
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        protected EntityPathSegment(EntityPathSegment next)
        {
            Next = next;
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return (obj is EntityPathSegment other) && Equals(other);
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public virtual bool Equals(EntityPathSegment other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}