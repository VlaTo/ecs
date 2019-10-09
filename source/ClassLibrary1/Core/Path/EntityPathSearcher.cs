using System;
using ClassLibrary1.Core.Extensions;
using ClassLibrary1.Core.Path.Extensions;
using ClassLibrary1.Core.Path.Segments;
using ClassLibrary1.Extensions;

namespace ClassLibrary1.Core.Path
{
    internal sealed class SearchResult
    {
        public static SearchResult Empty { get; } = new SearchResult
        {
            IsSuccess = true
        };

        public bool IsSuccess
        {
            get;
            private set;
        }

        public EntityBase Entity
        {
            get;
            private set;
        }

        private SearchResult()
        {
        }

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

    internal sealed class EntityPathSearcher
    {
        private readonly EntityBase entity;

        public EntityPathSearcher(EntityBase entity)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.entity = entity;
        }

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
            var found = SearchFrom(current, path.Entry.Next);

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

                if (segment.IsWildcard())
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

                throw new NotImplementedException();
                //break;
            }
        }
    }
}