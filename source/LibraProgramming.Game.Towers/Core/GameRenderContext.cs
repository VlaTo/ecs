using System.Numerics;
using Windows.UI;
using LibraProgramming.Ecs;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace LibraProgramming.Game.Towers.Core
{
    public class GameRenderContext : IGameRenderContext
    {
        private readonly CanvasDrawingSession session;

        public GameRenderContext(CanvasDrawingSession session)
        {
            this.session = session;
        }

        public void RenderEnemy(Vector2 position, EntityBase enemy)
        {
            using (var outline = new CanvasSolidColorBrush(session, Colors.Gray))
            {
                session.FillCircle(position, 8.0f, outline);
                session.DrawCircle(position, 8.0f, Colors.AntiqueWhite, 0.4f);
            }
        }
    }
}