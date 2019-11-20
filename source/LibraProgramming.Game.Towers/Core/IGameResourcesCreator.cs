using System;

namespace LibraProgramming.Game.Towers.Core
{
    public interface IGameResourcesCreator : IAsyncObservable<ICreateResourcesContext>
    {
    }
}