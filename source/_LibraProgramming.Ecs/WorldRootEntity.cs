using System;

namespace LibraProgramming.Ecs
{
    public sealed class WorldRootEntity : Entity
    {
        public WorldRootEntity()
            : base("Root")
        {
        }

        public WorldRootEntity(string key, EntityBase prototype) 
            : this()
        {
            throw new NotSupportedException();
        }

        public void Load(EntityState state)
        {

        }
    }
}