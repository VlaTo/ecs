using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ClassLibrary1.Core;
using ClassLibrary1.Extensions;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerTypeProxy(typeof(DebugEntityPathStringProxy))]
    public sealed class EntityPathString : IEquatable<EntityPathString>
    {
        public const char PathDelimiter = '/';

        public static readonly EntityPathString Empty;

        internal EntityPathStringSegment Entry
        {
            get;
        }

        internal EntityPathString(EntityPathStringSegment entry)
        {
            Entry = entry;
        }

        static EntityPathString()
        {
            Empty = new EntityPathString(null);
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
        public override int GetHashCode() => EntityPathStringComparer.Default.GetHashCode(this);

        public override string ToString()
        {
            var path = new StringBuilder();
            var current = Entry;

            while (null != current)
            {
                if (0 < path.Length && false == current.IsRoot())
                {
                    path.Append(PathDelimiter);
                }

                path.Append(current);

                current = current.Next;
            }

            return path.ToString();
        }

        public bool Equals(EntityPathString other) => EntityPathStringComparer.Default.Equals(this, other);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static EntityPathString Parse(string path)
        {
            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var parser = new EntityPathParser();
            var top = parser.Parse(path);

            if (null == top)
            {
                throw new Exception();
            }

            return new EntityPathString(top);
        }

        public static bool operator ==(EntityPathString left, EntityPathString right) =>
            EntityPathStringComparer.Default.Equals(left, right);

        public static bool operator !=(EntityPathString left, EntityPathString right) =>
            false == EntityPathStringComparer.Default.Equals(left, right);

        public static explicit operator string(EntityPathString instance) => instance.ToString();

        public static implicit operator EntityPathString(string path) => Parse(path);

        /// <summary>
        /// 
        /// </summary>
        internal sealed class DebugEntityPathStringProxy
        {
            public EntityPathStringSegment[] Segments
            {
                get;
            }

            public DebugEntityPathStringProxy(EntityPathString path)
            {
                var segments = new List<EntityPathStringSegment>();
                var entry = path.Entry;

                while (null != entry)
                {
                    segments.Add(entry);
                    entry = entry.Next;
                }

                Segments = segments.ToArray();
            }
        }
    }
}