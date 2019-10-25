using System.Numerics;
using LibraProgramming.Ecs;

namespace LibraProgramming.Game.Towers.Core
{
    public interface IGameRenderContext
    {
        void RenderEnemy(Vector2 position, EntityBase enemy);
    }
}