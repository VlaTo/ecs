﻿using System.Diagnostics;

namespace LibraProgramming.Ecs.Core.Path.Segments
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("//")]
    internal sealed class EntityPathRootSegment : EntityPathSegment
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public EntityPathRootSegment(EntityPathSegment next) 
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

            return other is EntityPathRootSegment;
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return EntityPath.PathDelimiter.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return new string(EntityPath.PathDelimiter, 2);
        }
    }
}