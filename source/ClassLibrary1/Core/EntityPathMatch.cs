using System;
using System.Collections.Generic;

namespace ClassLibrary1.Core
{
    internal sealed class EntityPathMatch : ICondition<IComponent>
    {
        private readonly Entity entity;
        private readonly MatchPattern pattern;

        public EntityPathMatch(EntityPathString path, Entity entity)
        {
            this.entity = entity;
            pattern = BuildPattern(path.Segments);
        }

        public bool IsMet(IComponent value)
        {
            var path = value.Entity.Path;
            return pattern.Match(path.Segments);
        }

        private static MatchPattern BuildPattern(IEnumerable<EntityPathStringSegment> segments)
        {
            var patters = new List<MatchPattern>();

            foreach (var segment in segments)
            {
                if (segment.IsWildCart)
                {
                    patters.Add(new WildcartMatch(WildCart.Star));
                    continue;
                }

                patters.Add(new ExactMatch(segment));
            }

            return patters.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        private abstract class MatchPattern
        {
            public MatchPattern Next
            {
                get;
            }

            protected MatchPattern(MatchPattern next)
            {
                Next = next;
            }

            public bool Match(IReadOnlyList<EntityPathStringSegment> segments)
            {
                throw new NotImplementedException();
            }

            protected abstract bool DoMatch();
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class ExactMatch : MatchPattern
        {
            private readonly EntityPathStringSegment segment;

            public ExactMatch(EntityPathStringSegment segment, MatchPattern next)
                : base(next)
            {
                this.segment = segment;
            }

            public override bool Match(EntityPathStringSegment segment)
            {
                return this.segment == segment&&Next.Match();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class WildcartMatch : MatchPattern
        {
            private readonly WildCart wildcart;

            public WildcartMatch(WildCart wildcart)
            {
                this.wildcart = wildcart;
            }

            public override bool Match(EntityPathStringSegment segment)
            {
                switch (wildcart)
                {
                    case WildCart.Star:
                    {
                        return true;
                    }

                    default:
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private enum WildCart
        {
            Star
        }
    }
}