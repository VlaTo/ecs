using System;
using System.Numerics;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Components;

namespace LibraProgramming.Game.Towers.Core
{
    public interface IEnemyMoveStrategy
    {
        Vector2 GetOrigin(PathComponent component);

        float CalculateAngle(Vector2 origin, Vector2 next);

        void Move(EntityBase entity, TimeSpan duration);
    }
}