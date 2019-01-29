using System;
using System.Collections.Generic;

namespace ClassLibrary1.Core
{
    /// <summary>
    /// Implements entity path match processor.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    ///     <listheader>
    ///         <term>path</term>
    ///         <description>meaning</description>
    ///     </listheader>
    ///     <item>
    ///         <term>
    ///             <c>/*</c>
    ///         </term>
    ///         <description>all entities, staring from root.</description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>/entity1</c>
    ///         </term>
    ///         <description>
    ///         all entities with key 'entity1', staring from root.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>/entity1/entity2</c>
    ///         </term>
    ///         <description>
    ///         all entities with key 'entity2' wich is child of entity 'entity1', staring from root.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>/entity1/*</c>
    ///         </term>
    ///         <description>
    ///             all entity with any key wich is child of entity 'entity1', staring from root.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>*</c>
    ///         </term>
    ///         <description>
    ///             all entities which is child of current entity.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>entity1</c>
    ///         </term>
    ///         <description>
    ///             all child entities with key 'entity1' from current.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>entity1/entity2</c>
    ///         </term>
    ///         <description>
    ///             all child entities with key 'entity1' from current.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>entity1/*</c>
    ///         </term>
    ///         <description>
    ///             all child entities with key 'entity1' from current.
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    internal sealed class EntityPathMatch : ICondition<Entity>
    {
        private readonly EntityPathString path;
        private readonly Entity entity;
        //private readonly MatchPattern pattern;

        public EntityPathMatch(EntityPathString path, Entity entity)
        {
            this.path = path;
            this.entity = entity;
        }

        public bool IsMet(Entity value)
        {
            return false;
        }

        /*private static MatchPattern BuildPattern(IEnumerable<EntityPathStringSegment> segments)
        {
            MatchPattern next = null;

            foreach (var segment in segments)
            {
                if (segment.IsWildCart)
                {
                    next = new WildcartMatch(WildCart.Star, next);
                    continue;
                }

                next = new SegmentMatch(segment, next);
            }

            return next;
        }*/

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

            public bool Match(IEnumerator<EntityPathStringSegment> segments)
            {
                //segments.Current;
                //segments.MoveNext();
                throw new NotImplementedException();
            }

            protected abstract bool DoMatch();
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class RootMatch : MatchPattern
        {
            public RootMatch(MatchPattern next)
                : base(next)
            {
            }

            protected override bool DoMatch()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class SegmentMatch : MatchPattern
        {
            private readonly EntityPathStringSegment segment;

            public SegmentMatch(EntityPathStringSegment segment, MatchPattern next)
                : base(next)
            {
                this.segment = segment;
            }

            protected override bool DoMatch()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class WildcartMatch : MatchPattern
        {
            private readonly WildCart wildcart;

            public WildcartMatch(WildCart wildcart, MatchPattern next)
                : base(next)
            {
                this.wildcart = wildcart;
            }

            protected override bool DoMatch()
            {
                throw new NotImplementedException();
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