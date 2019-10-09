namespace ClassLibrary1.Core.Path.Segments
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityPathWildCardSegment : EntityPathSegment
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public EntityPathWildCardSegment()
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

            return (other is EntityKeySegment) || (other is EntityPathWildCardSegment);
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