using System;
using System.Diagnostics;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{" + nameof(path) + "}")]
    public sealed class EntityPathString : IEquatable<EntityPathString>
    {
        private readonly string path;

        public EntityPathString(string path)
        {
            this.path = path;
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
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
            if (null == other)
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
            return null != left && left.Equals(right);
        }

        public static bool operator !=(EntityPathString left, EntityPathString right)
        {
            throw new NotImplementedException();
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