using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public class SpawningEnemies
    {
        private readonly Random random = new Random();
        private readonly Texture2D _enemyTexture;
        private Difficulty _difficulty = Difficulty.Easy;
        public Difficulty Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;

                switch (_difficulty)
                {
                    case Difficulty.Easy:
                        MaxEnemies = 3;
                        break;
                    case Difficulty.Medium:
                        MaxEnemies = 7;
                        break;
                    case Difficulty.Hard:
                        MaxEnemies = 12;
                        break;
                }
                hasSpawned = false;
            }
        }

        public int MaxEnemies { get; private set; } = 3;

        public float SafetyRad { get; set; } = TileMap.tilesize * 8f;

        public float RespawnDelaySeconds { get; set; } = 10f;

        private bool hasSpawned = false;
        private float respawnTimer = 0f;

        public SpawningEnemies(Texture2D enemyTexture, Difficulty difficulty = Difficulty.Easy)
        {
            _enemyTexture = enemyTexture;
            Difficulty = difficulty;
        }

        public void Update(TileMap map, Hero hero, List<Enemy> enemies)
        {
            if (!hasSpawned)
            {
                SpawnEnemies(map, hero, enemies);
                hasSpawned = true;
                respawnTimer = 0f;
                return;
            }
        }
        private void SpawnEnemies(TileMap map, Hero hero, List<Enemy> enemies)
        {

            int spawned = 0;
            int tries = 0;

            while (spawned < MaxEnemies && tries < 2000)
            {
                tries++;

                int x = random.Next(0, TileMap.Width);
                int y = random.Next(0, TileMap.Height);
                string key = $"{x};{y}";

                if (!map.IsWalkable(key))
                    continue;

                Vector2 pos = new Vector2(
                    x * TileMap.tilesize + TileMap.tilesize / 2f,
                    y * TileMap.tilesize + TileMap.tilesize / 2f
                );

                if (Vector2.Distance(pos, hero.Position) < SafetyRad)
                    continue;

                enemies.Add(new Enemy(50,_enemyTexture, pos));
                spawned++;
            }
        }
        public void ForceRespawn()
        {
            hasSpawned = false;
            respawnTimer = 0f;
        }
    }
}
