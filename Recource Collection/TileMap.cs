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
        public const int Width = 1000;
        public const int Height = 1000;


        public HashSet<TileType> SolidTiles = new HashSet<TileType>();
        public HashSet<TileType> FarmableTiles = new HashSet<TileType>
        {
            TileType.Tree1 , TileType.Tree2, TileType.Rock1 , TileType.Rock2 , TileType.bush1
        };
        public Dictionary<TileType, float> FarmTime = new Dictionary<TileType, float>();
        public Dictionary<TileType, Items> FarmDrops = new Dictionary<TileType, Items>();
        public Dictionary<TileType, int> DropAmount = new Dictionary<TileType, int>();
        public Dictionary<string, TileType> Tiles = new Dictionary<string, TileType>();
        public Dictionary<TileType, Texture2D> Assets;
        public TileMap(Dictionary<TileType, Texture2D> assets)
        {
            Assets = assets;

           

            FarmTime[TileType.bush1] = 1f;
            FarmTime[TileType.Tree1] = 1f;
            FarmTime[TileType.Tree2] = 1f;
            FarmTime[TileType.Rock1] = 1f;
            FarmTime[TileType.Rock2] = 1f;

            FarmDrops[TileType.bush1] = Items.berry;
            DropAmount[TileType.bush1] = 4;

            FarmDrops[TileType.Tree1] = Items.twigs;
            DropAmount[TileType.Tree1] = 2;
            
            FarmDrops[TileType.Tree2] = Items.twigs;
            DropAmount[TileType.Tree2] = 2;
            
            FarmDrops[TileType.Rock1] = Items.pebble;
            DropAmount[TileType.Rock1] = 4;
            
            FarmDrops[TileType.Rock2] = Items.pebble;
            DropAmount[TileType.Rock2] = 4;

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
            {
                return TileType.Void;
            }
                
            return Tiles[pos];
        }
        public void SetTile(string pos, TileType type)
        {
            if (InTileMap(pos))
                Tiles[pos] = type;
        }
        public bool IsWalkable(string pos)
        {
            if (!InTileMap(pos)) return false;
            return !SolidTiles.Contains(GetTile(pos));
        }

        public List<string> GetNeighbours(string pos, bool hideNonTraversables)
        {
            int[] coords = Array.ConvertAll(pos.Split(';'), int.Parse);
            List<string> neighbours = new List<string>();
            List<string> cardinal = new List<string>()
            {
                {$"{coords[0]};{coords[1]-1}"},
                {$"{coords[0]+1};{coords[1]}"},
                {$"{coords[0]};{coords[1]+1}"},
                {$"{coords[0]-1};{coords[1]}"}

            };

            List<string> notCardinal = new List<string>()
            {
                {$"{coords[0]-1};{coords[1]-1}"},
                {$"{coords[0]+1};{coords[1]-1}"},
                {$"{coords[0]-1};{coords[1]+1}"},
                {$"{coords[0]+1};{coords[1]+1}"}
            };

            foreach (string neighour in cardinal)
            {
                if (InTileMap(neighour))
                {
                    if (!(hideNonTraversables && SolidTiles.Contains(GetTile(neighour))))
                    {
                        neighbours.Add(neighour);
                    }
                        
                }
            }
            foreach (string neighbour in notCardinal)
            {
                if (InTileMap(neighbour))
                {
                    if (!(hideNonTraversables && SolidTiles.Contains(GetTile(neighbour))))
                    {
                        neighbours.Add(neighbour);
                    }
                        
                }
            }


            return neighbours;
        }
        public TileType this[string key]
        {
            get => GetTile(key);
            set => SetTile(key, value);
        }
    }
}
