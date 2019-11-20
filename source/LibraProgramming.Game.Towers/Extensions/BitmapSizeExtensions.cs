using System;
using System.Numerics;
using System.Text;
using Windows.Graphics.Imaging;

namespace LibraProgramming.Game.Towers.Extensions
{
    internal static class BitmapSizeExtensions
    {
        public static Vector2 ToVector2(this BitmapSize size) => new Vector2(size.Width, size.Height);

        public static string ToString(this BitmapSize size, IFormatProvider formatProvider)
        {
            return new StringBuilder()
                .AppendFormat(formatProvider, "D", size.Width)
                .Append(", ")
                .AppendFormat(formatProvider, "D", size.Height)
                .ToString();
        }
    }
}