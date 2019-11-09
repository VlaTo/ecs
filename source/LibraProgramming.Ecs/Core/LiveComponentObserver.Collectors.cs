using System;
using System.Collections.Generic;
using LibraProgramming.Ecs.Core.Reactive.Collections;

namespace LibraProgramming.Ecs.Core
{
    public partial class LiveComponentObserver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        private class FilteredEntityCollector<TComponent> : IScopedCollectionObserver<IComponent, EntityBase>
            where TComponent : IComponent
        {
            public IList<EntityBase> Entities
            {
                get;
            }

            public FilteredEntityCollector()
            {
                Entities = new List<EntityBase>();
            }

            public virtual ICollectionObserver<IComponent> CreateScope(EntityBase scope)
            {
                return new Scope(this, scope);
            }

            public void OnError(Exception error)
            {
                throw error;
            }

            public void OnCompleted()
            {
                ;
            }

            public void OnAdded(IComponent item)
            {
                var target = item.Entity;

                if (Entities.Contains(target))
                {
                    return;
                }

                if (FilterEntity(target))
                {
                    Entities.Add(target);
                }
            }

            public void OnRemoved(IComponent item)
            {
                var target = item.Entity;

                if (false == Entities.Contains(target))
                {
                    return;
                }

                if (false == FilterEntity(target))
                {
                    if (Entities.Remove(target))
                    {
                        ;
                    }
                }
            }

            protected virtual bool FilterEntity(EntityBase entity) => entity.Has<TComponent>();

            private void RemoveScope(EntityBase scope)
            {
                if (Entities.Remove(scope))
                {
                    ;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            private sealed class Scope : ICollectionObserver<IComponent>
            {
                private readonly FilteredEntityCollector<TComponent> collector;
                private readonly EntityBase scope;

                public Scope(FilteredEntityCollector<TComponent> collector, EntityBase scope)
                {
                    this.collector = collector;
                    this.scope = scope;
                }

                public void OnError(Exception error)
                {
                    collector.OnError(error);
                }

                public void OnCompleted()
                {
                    collector.RemoveScope(scope);
                }

                public void OnAdded(IComponent item)
                {
                    collector.OnAdded(item);
                }

                public void OnRemoved(IComponent item)
                {
                    collector.OnRemoved(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class FilteredEntityCollector<TComponent1, TComponent2> : FilteredEntityCollector<TComponent1>
            where TComponent1 : IComponent
            where TComponent2 : IComponent
        {
            protected override bool FilterEntity(EntityBase entity) =>
                base.FilterEntity(entity) && entity.Has<TComponent2>();
        }

        /// <summary>
        /// 
        /// </summary>
        private class FilteredEntityCollector<TComponent1, TComponent2, TComponent3> : FilteredEntityCollector<TComponent1, TComponent2>
            where TComponent1 : IComponent
            where TComponent2 : IComponent
            where TComponent3 : IComponent
        {
            protected override bool FilterEntity(EntityBase entity) =>
                base.FilterEntity(entity) && entity.Has<TComponent3>();
        }
    }
}