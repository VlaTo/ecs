using System;
using System.Numerics;
using System.Text;

namespace LibraProgramming.Game.Towers.Extensions
{
    internal static class Vector2Extensions
    {
        public static string ToString(this Vector2 value, IFormatProvider formatProvider)
        {
            var builder = new StringBuilder();
            
            PrintVector2(builder, value, formatProvider);
            
            return builder.ToString();
        }

        public static string ToString(this Vector2[] values, IFormatProvider formatProvider)
        {
            var builder = new StringBuilder();

            for (var index = 0; index < values.Length; index++)
            {
                if (0 < index)
                {
                    builder.Append("; ");
                }

                PrintVector2(builder, values[index], formatProvider);
            }

            return builder.ToString();
        }

        private static void PrintVector2(StringBuilder builder, Vector2 value, IFormatProvider formatProvider)
        {
            builder
                .AppendFormat(formatProvider, "F", value.X)
                .Append(", ")
                .AppendFormat(formatProvider, "F", value.Y);
        }
    }
}