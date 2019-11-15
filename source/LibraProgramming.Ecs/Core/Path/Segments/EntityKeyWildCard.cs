namespace LibraProgramming.Ecs.Core.Path.Segments
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityKeyWildCard : EntityPathSegment
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityKeyWildCard()
            : base(null)
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

            return (other is EntityKey) || (other is EntityKeyWildCard);
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