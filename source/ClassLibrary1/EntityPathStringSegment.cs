using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityPathStringSegment
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityPathStringSegment Next
        {
            get;
        }

        protected EntityPathStringSegment(EntityPathStringSegment next)
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

            return (obj is EntityPathStringSegment other) && Equals(other);
        }

        public virtual bool Equals(EntityPathStringSegment other)
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