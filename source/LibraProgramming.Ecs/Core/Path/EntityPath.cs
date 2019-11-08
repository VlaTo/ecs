using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LibraProgramming.Ecs.Core.Path.Segments;

namespace LibraProgramming.Ecs.Core.Path
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

        /// <inheritdoc cref="object.ToString" />
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

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(EntityPath left, EntityPath right) => EntityPathStringComparer.Default.Equals(left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EntityPath left, EntityPath right) => false == EntityPathStringComparer.Default.Equals(left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public static explicit operator string(EntityPath instance) => instance.ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static implicit operator EntityPath(string path) => Parse(path);

        /// <summary>
        /// 
        /// </summary>
        internal sealed class DebugEntityPathStringProxy
        {
            /// <summary>
            /// 
            /// </summary>
            public EntityPathSegment[] Segments
            {
                get;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
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