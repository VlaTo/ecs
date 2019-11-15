using System.Diagnostics;

namespace LibraProgramming.Ecs.Core.Path.Segments
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("..")]
    internal sealed class UpLevel : EntityPathSegment
    {
        public UpLevel(EntityPathSegment next)
            : base(next)
        {
        }

        /// <inheritdoc cref="EntityPathSegment.Equals(EntityPathSegment)" />
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

            return other is UpLevel;
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return new string('.', 2);
        }
    }
}