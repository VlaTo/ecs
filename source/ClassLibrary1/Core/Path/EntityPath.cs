using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ClassLibrary1.Core.Path.Segments;

namespace ClassLibrary1.Core.Path
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerTypeProxy(typeof(DebugEntityPathStringProxy))]
    public sealed partial class EntityPath : IEquatable<EntityPath>
    {
        public const char PathDelimiter = '/';

        public static readonly EntityPath Empty;

        internal EntityPathSegment Entry
        {
            get;
        }

        internal EntityPath(EntityPathSegment entry)
        {
            Entry = entry;
        }

        static EntityPath()
        {
            Empty = new EntityPath(null);
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

            return (obj is EntityPath other) && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode() => EntityPathStringComparer.Default.GetHashCode(this);

        public override string ToString()
        {
            var path = new StringBuilder();
            var current = Entry;

            while (null != current)
            {
                if (0 < path.Length && path[path.Length - 1] != PathDelimiter)
                {
                    path.Append(PathDelimiter);
                }

                path.Append(current);

                current = current.Next;
            }

            return path.ToString();
        }

        public bool Equals(EntityPath other) => EntityPathStringComparer.Default.Equals(this, other);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static EntityPath Parse(string path)
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

            return new EntityPath(top);
        }

        public static bool operator ==(EntityPath left, EntityPath right) =>
            EntityPathStringComparer.Default.Equals(left, right);

        public static bool operator !=(EntityPath left, EntityPath right) =>
            false == EntityPathStringComparer.Default.Equals(left, right);

        public static explicit operator string(EntityPath instance) => instance.ToString();

        public static implicit operator EntityPath(string path) => Parse(path);

        /// <summary>
        /// 
        /// </summary>
        internal sealed class DebugEntityPathStringProxy
        {
            public EntityPathSegment[] Segments
            {
                get;
            }

            public DebugEntityPathStringProxy(EntityPath path)
            {
                var segments = new List<EntityPathSegment>();
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