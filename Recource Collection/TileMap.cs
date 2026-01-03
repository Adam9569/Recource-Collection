using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Recource_Collection
{
    public class TileMap
    {

        public enum TileType
        {
            Water1,
            Water2,
            Tree1,
            Tree2,
            Rock1,
            Rock2,
            grass,
            bush1,
            sand1,
            Void,
        }

        public const int Chunksize = 320;
        public const int tilesize = 64;
        public const int Width = 512;
        public const int Height = 512;

        public HashSet<TileType> SolidTiles = new HashSet<TileType>();
        public Dictionary<string, TileType> Tiles = new Dictionary<string, TileType>();
        public Dictionary<TileType, Texture2D> Assets;
        public TileMap(Dictionary<TileType, Texture2D> assets)
        {
            Assets = assets;

            SolidTiles.Add(TileType.Water1);
            SolidTiles.Add(TileType.bush1);
        }
        public bool InTileMap(string key)
        {
            string[] p = key.Split(';');
            int x = int.Parse(p[0]);    
            int y = int.Parse(p[1]);

            if (x < 0 || y < 0) return false;
            if (x >= Width || y >= Height) return false;

            return true;
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
        public TileType GetTile(string pos)
        {
            if (!Tiles.ContainsKey(pos))
                return TileType.Void;
            return Tiles[pos];
        }
        public void SetTile(string pos, TileType type)
        {
            if (InTileMap(pos))
                Tiles[pos] = type;
        }
        public bool IsWalkable(string pos)
        {
            return !SolidTiles.Contains(GetTile(pos));
        }

    }
}
