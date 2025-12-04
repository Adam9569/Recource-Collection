using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Recource_Collection
{
    internal class TileMap
    {
        public Dictionary<string, string> Map = new Dictionary<string, string>();
        public const int tilesize = 64;
        public const int Chunksize = 320;
        public HashSet<string> SolidTiles = new HashSet<string>();
        public Dictionary<string, Texture2D> Assets { get; set; }
        public const int Width = 128;
        public const int Height = 128;
        public bool InTileMap(string key)
        {
            return Map.ContainsKey(key);
        }
        public string VectorToPosition(Vector2 position)
        {
            int x = (int)(position.X / tilesize);
            int y = (int)(position.Y / tilesize);
            string pos = $"{x};{y}";
            return pos;
        }
        public Vector2 PositionToVector(string pos)
        {
            int[] coords = Array.ConvertAll(pos.Split(';'), int.Parse);
            return new Vector2(coords[0] * tilesize, coords[1] * tilesize);
        }
        public string this[string key]
        {
            get => Map[key];
            set => Map.Add(key, value);
        }
        public TileMap(Dictionary<string, Texture2D> assets)
        {
            Assets = assets;
        }
    }
}
