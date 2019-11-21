using System;
using System.Numerics;
using System.Text;

namespace LibraProgramming.Game.Towers.Extensions
{
    internal static class SizeExtensions
    {
        public static Vector2 ToVector2(this System.Drawing.Size size) => new Vector2(size.Width, size.Height);

        public static string ToString(this System.Drawing.Size size, IFormatProvider formatProvider)
        {
            return new StringBuilder()
                .AppendFormat(formatProvider, "D", size.Width)
                .Append(", ")
                .AppendFormat(formatProvider, "D", size.Height)
                .ToString();
        }
    }
}