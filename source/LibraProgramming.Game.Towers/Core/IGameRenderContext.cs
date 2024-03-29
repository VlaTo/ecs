﻿using Windows.Foundation;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Game.Towers.Core
{
    public interface IGameRenderContext
    {
        CanvasDrawingSession DrawingSession
        {
            get;
        }

        Size CanvasSize
        {
            get;
        }
    }
}