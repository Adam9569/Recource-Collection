using System;
using static Recource_Collection.TileMap;

namespace Recource_Collection
{
    public static class WorldGen
    {
        public static void MapCreation(TileMap map, NoiseGen noise)
        {
            for (int x = 0; x < TileMap.Width;x++)
            {
                for (int y = 0; y < TileMap.Height;y++)
                {
                    string pos = $"{x};{y}";

                    float n = noise.Sample(x,y);

                    if (n < 0.35f)
                    {
                        float pick = noise.Sample(x+ 500, y+ 500);
                        map.SetTile(pos, pick > 0.5f ? TileType.Water2 : TileType.Water1);
                    }
                    else
                    {
                        map.SetTile(pos, TileType.grass);
                    }
                }
            }
            for (int x = 1; x < TileMap.Width -1;x++)
            {
                for (int y = 1; y < TileMap.Height -1;y++)
                {
                    string pos = $"{x};{y}";

                    if (map.GetTile(pos) == TileType.grass && NearWater(map, x, y))
                        map.SetTile(pos, TileType.sand1);
                }
            }
            for (int x = 0; x < TileMap.Width;x++)
            {
                for (int y = 0; y < TileMap.Height;y++)
                {
                    string pos = $"{x};{y}";
                    if (map.GetTile(pos) == TileType.grass)
                    {
                        float r = noise.Sample(x+ 1000, y+ 1000);

                        if (r > 0.75f)
                        {
                            float pick = noise.Sample(x+ 1100, y+ 1100);
                            map.SetTile(pos, pick > 0.5f ? TileType.Rock2 : TileType.Rock1);
                        }
                    }

                    
                }
            }
            for (int x = 0; x < TileMap.Width;x++)
            {
                for (int y = 0; y < TileMap.Height;y++)
                {
                    string pos = $"{x};{y}";
                    if (map.GetTile(pos) == TileType.grass)
                    {
                        float t = noise.Sample(x + 2000, y + 2000);

                        if (t > 0.68f)
                        {
                            float pick = noise.Sample(x + 2100, y + 2100);
                            map.SetTile(pos, pick > 0.5f ? TileType.Tree2 : TileType.Tree1);
                        }
                    }
                }
            }
            for (int x = 1; x < TileMap.Width -1; x++)
            {
                for (int y = 1; y < TileMap.Height -1; y++)
                {
                    string pos = $"{x};{y}";
                    if (map.GetTile(pos) == TileType.grass)
                    {
                        float b = noise.Sample(x + 3000, y + 3000);

                        if (b > 0.7f) 
                        {
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    string p2 = $"{x + dx};{y + dy}";
                                    if (map.GetTile(p2) == TileType.grass)
                                    {
                                        float place = noise.Sample(x + dx + 3100, y + dy + 3100);
                                        if (place > 0.65f)
                                            map.SetTile(p2, TileType.bush1);
                                    }
                                }
                            }
                        }
                    }
                }
                }
            }
        private static bool NearWater(TileMap map, int x, int y)
        {
            for (int dx = -1; dx <= 1;dx++)
            {
                for (int dy = -1; dy <= 1;dy++)
                {
                    var t = map.GetTile($"{x + dx};{y + dy}");
                    if (t == TileType.Water1 || t == TileType.Water2)
                        return true;
                }
            }
            return false;
        }
    }
}
