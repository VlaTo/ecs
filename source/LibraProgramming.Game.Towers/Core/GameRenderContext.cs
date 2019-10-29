using System.Collections.Generic;
using System.Numerics;

namespace LibraProgramming.Game.Towers.Core
{
    public class GameRenderContext : IGameRenderContext
    {
        private readonly IList<Vector2> renderList;

        public GameRenderContext(IList<Vector2> renderList)
        {
            this.renderList = renderList;
        }

        public void RenderEnemy(Vector2 position)
        {
            renderList.Add(position);
            /*using (var outline = new CanvasSolidColorBrush(session, Colors.Gray))
            {
                session.FillCircle(position, 8.0f, outline);
                session.DrawCircle(position, 8.0f, Colors.AntiqueWhite, 0.4f);
            }*/
        }
    }
}