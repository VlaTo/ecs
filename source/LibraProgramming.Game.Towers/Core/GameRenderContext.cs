
using Windows.Foundation;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Game.Towers.Core
{
    public class GameRenderContext : IGameRenderContext
    {
        public CanvasDrawingSession DrawingSession
        {
            get;
        }

        public Size CanvasSize
        {
            get;
        }

        public GameRenderContext(CanvasDrawingSession drawingSession, Size canvasSize)
        {
            DrawingSession = drawingSession;
            CanvasSize = canvasSize;
        }
    }
}