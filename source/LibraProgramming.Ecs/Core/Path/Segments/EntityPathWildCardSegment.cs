namespace LibraProgramming.Ecs.Core.Path.Segments
{
    internal sealed class EntityPathWildCardSegment : EntityPathSegment
    {
        public EntityPathWildCardSegment(EntityPathSegment next)
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

            return (other is EntityKeySegment) || (other is EntityPathWildCardSegment);
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public override string ToString()
        {
            return new string('*', 2);
        }
    }
}