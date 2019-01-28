using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StringEntityPathStringSegment : EntityPathStringSegment, IEquatable<StringEntityPathStringSegment>
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

        public bool Equals(StringEntityPathStringSegment other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return String.Equals(Segment, other.Segment);
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

            return (obj is StringEntityPathStringSegment other) && Equals(other);
        }

        public override int GetHashCode()
        {
            return (null != Segment ? Segment.GetHashCode() : 0);
        }
    }
}