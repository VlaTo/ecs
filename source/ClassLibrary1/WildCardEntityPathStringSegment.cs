using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WildCardEntityPathStringSegment : EntityPathStringSegment
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public WildCardEntityPathStringSegment()
            : base(null)
        {
        }

        public override bool Equals(EntityPathStringSegment other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (other is StringEntityPathStringSegment) || (other is WildCardEntityPathStringSegment);
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