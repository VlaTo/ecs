using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace LibraProgramming.Game.Towers.Core
{
    internal static class VectorConverter
    {
        public static Vector2 FromString(string value)
        {
            var values = value.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);

            if (0 == values.Length)
            {
                return Vector2.Zero;
            }

            var culture = CultureInfo.InvariantCulture;
            var temp = values
                .Select(val => System.Single.Parse(val, culture))
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
    }
}