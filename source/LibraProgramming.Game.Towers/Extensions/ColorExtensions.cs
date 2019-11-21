using System.Numerics;
using Windows.UI;

namespace LibraProgramming.Game.Towers.Extensions
{
    internal static  class ColorExtensions
    {
        public static Vector4 ToVector4(this Color color)
        {
            const float nominal = 255.0f;
            return new Vector4(
                color.R / nominal,
                color.G / nominal,
                color.B / nominal,
                color.A / nominal
            );
        }
    }
}