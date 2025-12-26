using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Recource_Collection
{
    public class NoiseGen
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
                noiseHeight += perlinValue * amplitude;
            }
            float normalized = (noiseHeight / maxAmplitude + 1f) / 2f;
            return Math.Clamp(normalized, 0f, 1f);
        }

        private float Perlin(float x, float y)
        {
            int xCoord = (int)MathF.Floor(x) & 31;
            int yCoord = (int)MathF.Floor(y) & 31;

            float RemX = x - MathF.Floor(x);
            float RemY = y - MathF.Floor(y);

            float u = fade(RemX);
            float v = fade(RemY);

            int tL = Permutation[(Permutation[xCoord] + yCoord) & 31];
            int tR = Permutation[(Permutation[xCoord] + yCoord + 1) & 31];
            int bL = Permutation[(Permutation[xCoord + 1 & 31] + yCoord) & 31];
            int bR = Permutation[(Permutation[xCoord + 1 & 31] + yCoord + 1) & 31];

            float x1 = Lerp(
                Grad(tL, RemX, RemY),
                Grad(tR, RemX - 1, RemY),
                u);
            float x2 = Lerp(
                Grad(bL, RemX, RemY - 1),
                Grad(bR, RemX, RemY - 1),
                u);
            return Lerp(x1, x2,v);


        }
        float fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }
        private float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

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