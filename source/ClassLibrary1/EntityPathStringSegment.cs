using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public sealed class EntityPathStringSegment : IEquatable<EntityPathStringSegment>
    {
        private readonly string str;

        public bool IsWildCart => 1 == str.Length && '*' == str[0];

        public EntityPathStringSegment(string str)
        {
            this.str = str;
        }

        public bool Equals(EntityPathStringSegment other)
        {
            EntityPathStringSegment
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is EntityPathStringSegment other && Equals(other);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static implicit operator string(EntityPathStringSegment segment)
        {
            return segment.str;
        }
    }
}