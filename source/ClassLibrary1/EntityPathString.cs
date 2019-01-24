using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{" + nameof(path) + "}")]
    public sealed class EntityPathString : IEquatable<EntityPathString>
    {
        private readonly string path;

        internal IEnumerable<EntityPathStringSegment> Segments
        {
            get
            {
                var segments = new List<EntityPathStringSegment>();

                foreach (var str in path.Split(Entity.Separator, StringSplitOptions.None))
                {
                    if (String.IsNullOrWhiteSpace(str))
                    {
                        ;
                    }

                    segments.Add(new EntityPathStringSegment(str));
                }

                return segments.ToArray();
            }
        }

        public EntityPathString(string path)
        {
            this.path = path;
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

            return (obj is EntityPathString other) && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return path.GetHashCode();
        }

        public override string ToString()
        {
            return '{' + path + '}';
        }

        public bool Equals(EntityPathString other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return path.Equals(other.path);
        }

        public static bool operator ==(EntityPathString left, EntityPathString right)
        {
            return false == ReferenceEquals(null, left) && left.Equals(right);
        }

        public static bool operator !=(EntityPathString left, EntityPathString right)
        {
            return ReferenceEquals(null, left) || false == left.Equals(right);
        }

        public static explicit operator string(EntityPathString instance)
        {
            return instance.path;
        }

        public static implicit operator EntityPathString(string path)
        {
            return new EntityPathString(path);
        }
    }
}