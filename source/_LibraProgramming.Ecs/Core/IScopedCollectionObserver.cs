using LibraProgramming.Ecs.Core.Reactive.Collections;

namespace LibraProgramming.Ecs.Core
{
    public interface IScopedCollectionObserver<in TEntity, in TScope> : ICollectionObserver<TEntity>
    {
        ICollectionObserver<TEntity> CreateScope(TScope scope);
    }
}