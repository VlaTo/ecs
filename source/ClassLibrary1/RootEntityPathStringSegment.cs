using System;
using System.Diagnostics;

namespace ClassLibrary1
{
    [DebuggerDisplay("/")]
    public sealed class RootEntityPathStringSegment : EntityPathStringSegment, IEquatable<RootEntityPathStringSegment>
    {
        public RootEntityPathStringSegment(EntityPathStringSegment next) 
            : base(next)
        {
        }

        public bool Equals(RootEntityPathStringSegment other)
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

            return (obj is RootEntityPathStringSegment other) && Equals(other);
        }

        public override int GetHashCode()
        {
            return EntityPathString.PathDelimiter.GetHashCode();
        }

        public override string ToString()
        {
            return new string(EntityPathString.PathDelimiter, 1);
        }
    }
}