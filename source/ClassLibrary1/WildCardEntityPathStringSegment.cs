using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WildCardEntityPathStringSegment : EntityPathStringSegment, IEquatable<WildCardEntityPathStringSegment>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public WildCardEntityPathStringSegment()
            : base(null)
        {
        }

        public bool Equals(WildCardEntityPathStringSegment other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return true;
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

            return (obj is WildCardEntityPathStringSegment other) && Equals(other);
        }

        public override int GetHashCode()
        {
            return '*'.GetHashCode();
        }

        public override string ToString()
        {
            return new string('*', 1);
        }
    }
}