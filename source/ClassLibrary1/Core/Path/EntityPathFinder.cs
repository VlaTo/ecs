using System;
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

    internal sealed class EntityPathFinder
    {
        private readonly EntityBase entity;

        public EntityPathFinder(EntityBase entity)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.entity = entity;
        }

        /*public FindResult FindOne(EntityPathString path)
        {
            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path == EntityPathString.Empty)
            {
                return FindResult.Empty;
            }

            adapter.Reset();

            if (false == adapter.NextSibling())
            {
                throw new Exception();
            }

            Search(path.Entry);
        }*/

        public SearchResult Search(EntityPath path)
        {
            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path == EntityPath.Empty)
            {
                return SearchResult.Empty;
            }

            var current = path.Entry.IsRoot() ? entity.Root : entity;
            var found = SearchFromCurrent(current, path.Entry.Next);

            return SearchResult.Success(found);
        }

        private static EntityBase SearchFromCurrent(EntityBase entity, EntityPathSegment segment)
        {
            while (true)
            {
                if (segment.IsString(out var str))
                {
                    var index = entity.Children.FindIndex(current => current == str.Segment);

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

                if (segment.IsWildcard(out var wildcard))
                {
                    if (null == segment.Next)
                    {
                        return entity.Children.Count > 0 ? entity.Children[0] : null;
                    }

                    foreach (var child in entity.Children)
                    {
                        var temp = SearchFromCurrent(child, segment.Next);

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