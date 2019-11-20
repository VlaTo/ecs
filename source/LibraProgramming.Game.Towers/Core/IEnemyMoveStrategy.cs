using LibraProgramming.Ecs;
using System;

namespace LibraProgramming.Game.Towers.Core
{
    public interface IEnemyMoveStrategy
    {
        void PlaceEnemy(EntityBase enemy);

        void MoveEnemy(EntityBase entity, TimeSpan duration);
    }
}