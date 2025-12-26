using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recource_Collection.TileMap;

namespace Recource_Collection
{
    public static class WorldGen
    {
        public static void MapCreation(TileMap map, NoiseGen noise)
        {
            for (int x = 0; x < TileMap.Width; x++)
            {
                for (int y = 0; y < TileMap.Height; y++)
                {
                    float n = noise.Sample(x, y);
                    string pos = $"{x};{y}";

                    if (n < 0.3f)
                        map.SetTile(pos, TileType.Water1);
                    else if (n < 0.45f)
                        map.SetTile(pos, TileType.Rock1);
                    else if (n < 0.7f)
                        map.SetTile(pos, TileType.grass);
                    else
                        map.SetTile(pos, TileType.Tree1);
                }
            }
        }
    }
}
