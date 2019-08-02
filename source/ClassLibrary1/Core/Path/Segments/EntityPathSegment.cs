using System;

namespace ClassLibrary1.Core.Path.Segments
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class EntityPathSegment
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityPathSegment Next
        {
            get;
        }

        protected EntityPathSegment(EntityPathSegment next)
        {
            Next = next;
        }

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
    }
}