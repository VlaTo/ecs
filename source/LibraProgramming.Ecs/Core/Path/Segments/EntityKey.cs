using System;

namespace LibraProgramming.Ecs.Core.Path.Segments
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityKey : EntityPathSegment
    {
        public string Key
        {
            get;
        }

        public EntityKey(string key, EntityPathSegment next)
            : base(next)
        {
            Key = key;
        }

        public override string ToString()
        {
            return Key;
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

            return (other is EntityKey str) && String.Equals(Key, str.Key)
                   || (other is EntityKeyWildCard);
        }

        public override int GetHashCode()
        {
            return (null != Key ? Key.GetHashCode() : 0);
        }
    }
}