using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    n = Math.Clamp(n, 0f, 1f);

                    n = MathF.Pow(n, 0.7f);

                    if (n < 0.12f)
                        map.SetTile($"{x};{y}", TileType.Water1);
                    else if (n < 0.20f)
                        map.SetTile($"{x};{y}", TileType.sand1);
                    else if (n < 0.55f)
                        map.SetTile($"{x};{y}", TileType.grass);
                    else if (n < 0.78f)
                        map.SetTile($"{x};{y}", TileType.Tree1);
                    else
                        map.SetTile($"{x};{y}", TileType.Rock1);
                }
            }
        }
    }
}
