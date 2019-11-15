using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using Windows.Networking.NetworkOperators;

namespace LibraProgramming.Game.Towers.Core
{
    internal static class VectorConverter
    {
        public static Vector2 FromString(string value)
        {
            return FromString(value, CultureInfo.InvariantCulture);
        }

        public static Vector2 FromString(string value, IFormatProvider formatProvider)
        {
            var values = value.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);

            if (0 == values.Length)
            {
                return Vector2.Zero;
            }

            var temp = values
                .Select(val => System.Single.Parse(val, formatProvider))
                .ToArray();

            if (1 == temp.Length)
            {
                return new Vector2(temp[0]);
            }

            if (2 == temp.Length)
            {
                return new Vector2(temp[0], temp[1]);
            }

            throw new InvalidOperationException();
        }

        public static string ToString(Vector2 value, IFormatProvider formatProvider)
        {
            return new StringBuilder()
                .AppendFormat(formatProvider, "F", value.X)
                .Append(',')
                .AppendFormat(formatProvider, "F", value.Y)
                .ToString();
        }
    }
}