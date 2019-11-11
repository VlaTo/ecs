using System;

namespace LibraProgramming.Ecs.Core.Path.Segments
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityKeySegment : EntityPathSegment
    {
        public string Key
        {
            get;
        }

        public EntityKeySegment(string key, EntityPathSegment next)
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

            return (other is EntityKeySegment str) && String.Equals(Key, str.Key)
                   || (other is EntityKeyWildCardSegment);
        }

        public override int GetHashCode()
        {
            return (null != Key ? Key.GetHashCode() : 0);
        }
    }
}