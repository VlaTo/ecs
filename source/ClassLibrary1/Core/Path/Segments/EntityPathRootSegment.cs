using System.Diagnostics;

namespace ClassLibrary1.Core.Path.Segments
{
    [DebuggerDisplay("//")]
    internal sealed class EntityPathRootSegment : EntityPathSegment
    {
        public EntityPathRootSegment(EntityPathSegment next) 
            : base(next)
        {
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

            return other is EntityPathRootSegment;
        }

        public override int GetHashCode()
        {
            return EntityPath.PathDelimiter.GetHashCode();
        }

        public override string ToString()
        {
            return new string(EntityPath.PathDelimiter, 2);
        }
    }
}