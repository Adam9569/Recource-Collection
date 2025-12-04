using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Recource_Collection
{
    internal class NoiseGen
    {
        private int Seed;
        private float Scale;
        private int Octaves;
        private float Persistence;
        private float Sharpness;

        public NoiseGen(int seed, float scale = 0.05f, int octaves = 3, float persistence = 0.5f, float sharpness = 2.0f)
        {
            Seed = seed;
            Scale = scale;
            Octaves = octaves;
            Persistence = persistence;
            Sharpness = sharpness;

        }

        public float Sample(float x, float y)
        {
            float amplitude = 1f;
            float frequency = 1f;
            float noiseHeight = 0f;
            float maxAmplitude = 0f;

            for (int i = 0; i < Octaves; i++)
            {
                float sampleX = (x*Scale*frequency) + Seed;
                float sampleY = (y * Scale * frequency) + Seed;
                float perlinValue = (Perlin(sampleX, sampleY) * 2F) - 1f;

                maxAmplitude += amplitude;
                amplitude *= Persistence;
                frequency *= Sharpness;
            }
            float normalized = (noiseHeight / maxAmplitude + 1f) / 2f;
            return Math.Clamp(normalized, 0f, 1f);
        }

        private float Perlin(float x, float y)
        {
            int X = (int)MathF.Floor(x) & 255; int Y = (int)MathF.Floor(y) & 255; x -= MathF.Floor(x);
            y -= MathF.Floor(y); float u = Fade(x);
            float v = Fade(y);
            int A = (Permutation[X] + Y) & 255;
            int B = (Permutation[X + 1] + Y) & 255;
            float res = Lerp(v, Lerp(u, Grad(Permutation[A], x, y), Grad(Permutation[B], x - 1, y)), Lerp(u, Grad(Permutation[A + 1], x, y - 1), Grad(Permutation[B + 1], x - 1, y - 1)));
            return (res + 1f) / 2f;
        }

        private float Fade(float t) 
        { return t * t * t * (t * (t * 6 - 15) + 10); }

        private float Lerp(float t, float a, float b) 
        { return a + t * (b - a); }
        private float Grad(int hash, float x, float y)
        {
            int h = hash & 7; 
            float u = h < 4 ? x : y;
            float v = h < 4 ? y : x; 
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
        private static readonly int[] Permutation = new int[32] 
        { 12, 5, 7, 14, 3, 8, 11, 0, 1, 9, 2, 6, 13, 10, 15, 4,
          12, 5, 7, 14, 3, 8, 11, 0, 1, 9, 2, 6, 13, 10, 15, 4
        };
    }
}