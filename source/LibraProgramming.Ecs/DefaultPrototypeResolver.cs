using LibraProgramming.Ecs.Core.Path;
using System;

namespace LibraProgramming.Ecs
{
    public partial class EntityFactory
    {
        internal class DefaultPrototypeResolver : IPrototypeResolver
        {
            private EntityState cache;

            public DefaultPrototypeResolver()
            {
            }

            public void Initialize(EntityState state)
            {
                cache = state;
            }

            public EntityBase Resolve(EntityPath path)
            {
                if (null == path)
                {
                    throw new ArgumentNullException(nameof(path));
                }

                //var adapter = new EntityStatePathAdapter(cache);
                //var finder = new EntityPathFinder(adapter);

                //return finder.FindOne(path);

                throw new NotImplementedException();
            }
        }
    }
}