using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Windows.Graphics.Imaging;

namespace LibraProgramming.Game.Towers.Core
{
    internal static class Converters
    {
        public static readonly Converter<string, Vector2> Vector2;

        public static readonly Converter<string, int[]> Tiles;

        public static readonly Converter<string, Size> Size;

        public static readonly Converter<string, Vector2[]> Vectors;

        public static readonly Converter<string, BitmapSize> BitmapSize;

        public static readonly Converter<string, Matrix3x2> Matrix3x2;

        static Converters()
        {
            var formatProvider = CultureInfo.InvariantCulture;

            Vector2 = value => ParseVector2(value, formatProvider);
            Tiles = value => ParseTiles(value, formatProvider);
            Size = value => ParseSize(value, formatProvider);
            BitmapSize = value => ParseBitmapSize(value, formatProvider);
            Matrix3x2 = value => ParseMatrix3x2(value, formatProvider);
            Vectors = value => ParseVectors(value, formatProvider);
        }

        private static Vector2 ParseVector2(string value, IFormatProvider formatProvider)
        {
            var values = value.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (0 == values.Length)
            {
                return System.Numerics.Vector2.Zero;
            }

            var numbers = values
                .Select(number => System.Single.Parse(number, formatProvider))
                .ToArray();

            if (1 == numbers.Length)
            {
                return new Vector2(numbers[0]);
            }

            if (2 == numbers.Length)
            {
                return new Vector2(numbers[0], numbers[1]);
            }

            throw new InvalidOperationException();
        }

        private static int[] ParseTiles(string value, IFormatProvider formatProvider)
        {
            return value
                .Trim()
                .Split(new[] { ' ', ',', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(cell => Int32.Parse(cell, formatProvider))
                .ToArray();
        }

        private static Size ParseSize(string value, IFormatProvider formatProvider)
        {
            var values = value.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (0 == values.Length)
            {
                return System.Drawing.Size.Empty;
            }

            var numbers = values
                .Select(number => System.Int32.Parse(number, formatProvider))
                .ToArray();

            if (1 == numbers.Length)
            {
                return new Size(numbers[0], numbers[0]);
            }

            if (2 == numbers.Length)
            {
                return new Size(numbers[0], numbers[1]);
            }

            throw new InvalidOperationException();
        }

        private static Vector2[] ParseVectors(string value, IFormatProvider formatProvider)
        {
            var values = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (0 == values.Length)
            {
                return Array.Empty<Vector2>();
            }

            return values
                .Select(number => ParseVector2(number, formatProvider))
                .ToArray();
        }

        private static BitmapSize ParseBitmapSize(string value, IFormatProvider formatProvider)
        {
            var values = value.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (0 == values.Length)
            {
                return new BitmapSize();
            }

            var numbers = values
                .Select(number => UInt32.Parse(number, formatProvider))
                .ToArray();

            if (1 == numbers.Length)
            {
                return new BitmapSize
                {
                    Width = numbers[0],
                    Height = numbers[0]
                };
            }

            if (2 == numbers.Length)
            {
                return new BitmapSize
                {
                    Width = numbers[0],
                    Height = numbers[1]
                };
            }

            throw new InvalidOperationException();

        }

        private static Matrix3x2 ParseMatrix3x2(string value, IFormatProvider formatProvider)
        {
            var values = value.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);

            if (0 == values.Length)
            {
                return new Matrix3x2();
            }

            var numbers = values
                .Select(number => System.Single.Parse(number, formatProvider))
                .ToArray();

            if (1 == numbers.Length)
            {
                var val = numbers[0];

                if (val.Equals(1.0f))
                {
                    return System.Numerics.Matrix3x2.Identity;
                }

                throw new InvalidOperationException();
            }

            if (6 == numbers.Length)
            {
                return new Matrix3x2(
                    numbers[0], numbers[1], numbers[2],
                    numbers[3], numbers[4], numbers[5]
                );
            }

            throw new InvalidOperationException();

        }
    }
}