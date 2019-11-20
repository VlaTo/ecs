using System;
using System.Numerics;
using Windows.Foundation;
using LibraProgramming.Game.Towers.Components;

namespace LibraProgramming.Game.Towers.Extensions
{
    internal static class TileSizeExtensions
    {
        public static Size ToSize(this TileSize size) => new Size(size.Width, size.Height);
        
        public static Vector2 ToVector2(this TileSize size) => new Vector2(size.Width, size.Height);
    }
}