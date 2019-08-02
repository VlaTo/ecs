using System;

namespace ClassLibrary1.Core.Path.Segments
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityPathStringSegment : EntityPathSegment
    {
        public string Segment
        {
            get;
        }

        public EntityPathStringSegment(string segment, EntityPathSegment next)
            : base(next)
        {
            Segment = segment;
        }

        public override string ToString()
        {
            return Segment;
        }

        public override bool Equals(EntityPathSegment other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (other is EntityPathStringSegment str) && String.Equals(Segment, str.Segment)
                   || (other is EntityPathWildCardSegment);
        }

        public override int GetHashCode()
        {
            return (null != Segment ? Segment.GetHashCode() : 0);
        }
    }
}