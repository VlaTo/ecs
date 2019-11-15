namespace LibraProgramming.Ecs.Core.Path.Segments
{
    internal sealed class PathWildCard : EntityPathSegment
    {
        public PathWildCard(EntityPathSegment next)
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

            return (other is EntityKey) || (other is PathWildCard);
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public override string ToString()
        {
            return new string('*', 2);
        }
    }
}