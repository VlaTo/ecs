using System;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace LibraProgramming.Game.Towers.Core.Particles
{
    public class Particle
    {
        private readonly float speed;
        private bool fall = false;
        private Transform2DEffect turbulence;
        private DisplacementMapEffect displacement;

        private static readonly Random generator = new Random(DateTime.Now.Millisecond);

        public int particleBitmap;

        public float Radius
        {
            get; 
            set;
        }

        public Vector2 Position
        {
            get; 
            set;
        }

        public Vector2 Orientation
        {
            get; 
            set;
        }

        public Matrix3x2 TurbulenceMatrix;

        public float random;
        public float factor;

        public Particle(
            Vector2 position, 
            int idxBitmap, 
            Vector2 orientation,
            //float orientationx, 
            //float orientationy, 
            CanvasRenderTarget source, 
            float particlespeed = 0.25f, 
            bool particlefall = false)
        {
            particleBitmap = idxBitmap;
            Radius = (particleBitmap + 1) * 4;
            Position = position;
            Orientation = orientation;

            //Here is all the turbulence, without this is linear
            random = generator.Next(2, 50);
            factor = 0.7f * random / 10.0f;

            var octaves = generator.Next(1, 5);

            turbulence = new Transform2DEffect
            {
                Source = new TurbulenceEffect
                {
                    Octaves = octaves
                }
            };

            displacement = new DisplacementMapEffect
            {
                Source = source, 
                Displacement = turbulence
            };

            speed = particlespeed;
            fall = particlefall;
        }

        private void UpdatePosition()
        {
            Position += Vector2.Multiply(Orientation, speed);
        }

        private void UpdateTurbulence(float elapsed)
        {
            var x = (float) Math.Cos(elapsed + random) * 2 - 2;
            var y = (float) Math.Sin(elapsed) * 2 - 2;

            TurbulenceMatrix = Matrix3x2.CreateTranslation(x, y);
            turbulence.TransformMatrix = TurbulenceMatrix;
            displacement.Amount = (float)Math.Sin(elapsed * factor) * random;
        }
    }
}