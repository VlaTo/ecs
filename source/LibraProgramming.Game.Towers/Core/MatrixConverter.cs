using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace LibraProgramming.Game.Towers.Core
{
    internal static class MatrixConverter
    {
        public static Matrix3x2 FromString(string value)
        {
            var values = value.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);

            if (0 == values.Length)
            {
                return new Matrix3x2();
            }

            var culture = CultureInfo.InvariantCulture;
            var temp = values
                .Select(val => System.Single.Parse(val, culture))
                .ToArray();

            if (1 == temp.Length)
            {
                var val = temp[0];

                if (val.Equals(1.0f))
                {
                    return Matrix3x2.Identity;
                }

                throw new InvalidOperationException();
            }

            if (6 == temp.Length)
            {
                return new Matrix3x2(
                    temp[0], temp[1], temp[2],
                    temp[3], temp[4], temp[5]
                );
            }

            throw new InvalidOperationException();
        }
    }
}