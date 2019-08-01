using System;

namespace ClassLibrary1
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

            public EntityBase Resolve(EntityPathString path)
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