using System;

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
            throw new NotImplementedException();
        }
    }
}