using System.Diagnostics;

namespace ClassLibrary1
{
    [DebuggerDisplay("/")]
    public sealed class RootEntityPathStringSegment : EntityPathStringSegment
    {
        public RootEntityPathStringSegment(EntityPathStringSegment next) 
            : base(next)
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

            return other is RootEntityPathStringSegment;
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