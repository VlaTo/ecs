using System;
using System.Linq;

namespace LibraProgramming.Game.Towers.Extensions
{
    internal static class Int32ArrayExtensions
    {
        public static string ToString(this int[] values, IFormatProvider formatProvider)
        {
            return String.Join(' ', values.Select(value => value.ToString("D", formatProvider)));
        }
    }
}