using System;
using System.Numerics;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Components;

namespace LibraProgramming.Game.Towers.Core
{
    internal sealed class EnemyMoveByPathStrategy : IEnemyMoveStrategy
    {
        [PrefferedConstructor]
        public EnemyMoveByPathStrategy()
        {
        }

        public Vector2 GetOrigin(PathComponent component)
        {
            if (0 == component.WayPoints.Length)
            {
                return Vector2.Zero;
            }

            return component.WayPoints[0];
        }

        public float CalculateAngle(Vector2 origin, Vector2 next)
        {
            return Vector2.Dot(origin, next);
        }

        public void Move(EntityBase entity, TimeSpan duration)
        {
            var position = entity.Get<PositionComponent>();
            var move = entity.Get<MoveComponent>();

            UpdatePosition(position, move, (float) duration.TotalSeconds);
        }
        
        private static void UpdatePosition(PositionComponent positionComponent, MoveComponent moveComponent, float duration)
        {
            var distance = new Vector2(MathF.Cos(moveComponent.Angle), MathF.Sin(moveComponent.Angle)) * (moveComponent.Speed * duration);
            var position = positionComponent.Position + distance;
            var viewport = GetViewport(moveComponent.Entity);

            if (null != viewport)
            {
                if (position.X < viewport.Horizontal.Min)
                {
                    position.X = viewport.Horizontal.Max;
                }
                else if (position.X > viewport.Horizontal.Max)
                {
                    position.X = viewport.Horizontal.Min;
                }

                if (position.Y < viewport.Vertical.Min)
                {
                    position.Y = viewport.Vertical.Max;
                }
                else if (position.Y > viewport.Vertical.Max)
                {
                    position.Y = viewport.Vertical.Min;
                }
            }

            positionComponent.Position = position;
        }

        private static ViewportComponent GetViewport(EntityBase entity)
        {
            var parent = entity;

            while (null != parent)
            {
                if (parent.Has<ViewportComponent>())
                {
                    return parent.Get<ViewportComponent>();
                }

                parent = parent.Parent;
            }

            return null;
        }
    }
}