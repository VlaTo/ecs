using LibraProgramming.Ecs.Core.Extensions;
using LibraProgramming.Ecs.Core.Path.Extensions;
using LibraProgramming.Ecs.Core.Path.Segments;
using System;

namespace LibraProgramming.Ecs.Core.Path
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class SearchResult
    {
        /// <summary>
        /// 
        /// </summary>
        public static SearchResult Empty { get; } = new SearchResult
        {
            IsSuccess = true
        };

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityBase Entity
        {
            get;
            private set;
        }

        private SearchResult()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static SearchResult Success(EntityBase entity)
        {
            var result = new SearchResult
            {
                IsSuccess = null != entity,
                Entity = entity
            };

            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityPathSearcher
    {
        private readonly EntityBase entity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public EntityPathSearcher(EntityBase entity)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.entity = entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public SearchResult Find(EntityPath path)
        {
            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path == EntityPath.Empty)
            {
                return SearchResult.Empty;
            }

            var current = path.IsAbsolute() ? entity.Root : entity;
            var found = SearchFrom(current, path.IsAbsolute() ? path.Entry.Next : path.Entry);

            return SearchResult.Success(found);
        }

        private static EntityBase SearchFrom(EntityBase entity, EntityPathSegment segment)
        {
            while (true)
            {
                if (segment.IsEntityKey(out var key))
                {
                    var index = entity.Children.FindIndex(child => child.Key == key);

                    if (0 > index)
                    {
                        return null;
                    }

                    if (null == segment.Next)
                    {
                        return entity.Children[index];
                    }

                    entity = entity.Children[index];
                    segment = segment.Next;

                    continue;
                }

                if (segment.IsWildCard())
                {
                    if (null == segment.Next)
                    {
                        return entity.Children.Count > 0 ? entity.Children[0] : null;
                    }

                    foreach (var child in entity.Children)
                    {
                        var temp = SearchFrom(child, segment.Next);

                        if (null != temp)
                        {
                            return temp;
                        }
                    }

                    return null;
                }

                if (segment.IsUpLevel())
                {
                    if (null == entity.Parent)
                    {
                        throw new Exception();
                    }

                    entity = entity.Parent;
                    segment = segment.Next;

                    continue;
                }

                throw new NotImplementedException();
            }
        }
    }
}