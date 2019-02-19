using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StringEntityPathStringSegment : EntityPathStringSegment
    {
        public string Segment
        {
            get;
        }

        public StringEntityPathStringSegment(string segment, EntityPathStringSegment next)
            : base(next)
        {
            Segment = segment;
        }

        public override string ToString()
        {
            return Segment;
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

            return (other is StringEntityPathStringSegment str) && String.Equals(Segment, str.Segment)
                   || (other is WildCardEntityPathStringSegment);
        }

        public override int GetHashCode()
        {
            return (null != Segment ? Segment.GetHashCode() : 0);
        }
    }
}