using System;
using System.Text;

namespace LibraProgramming.Game.Towers.Extensions
{
    internal static class SizeExtensions
    {
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