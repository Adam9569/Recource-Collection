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

    public enum EnemyType
    { 
        Rabbit,
        Goblin
    }

    public class SpawningEnemies
    {
        private List<EnemyType> enemyTypes = new List<EnemyType>();
        private readonly Random random = new Random();
        private Texture2D goblinTexture;
        private Texture2D rabbitTexture;
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;

        private void SetDifficulty()
        {
            switch (Difficulty)
            {
                case Difficulty.Easy:
                    baseMaxEnemies = 3;
                    break;
                case Difficulty.Medium:
                    baseMaxEnemies = 5;
                    break;
                case Difficulty.Hard:
                    baseMaxEnemies = 7;
                    break;
            }

            MaxEnemies = baseMaxEnemies;
        }

        public int MaxEnemies { get; private set; } = 3;
        private int baseMaxEnemies = 3;
        public int NightMultiplier { get; set; } = 2;

        public float Rad { get; set; } = TileMap.tilesize * 8f;

        private bool hasSpawned = false;
        private float respawnTimer = 0f;

        public SpawningEnemies(Texture2D goblinTex, Texture2D rabbitTex)
        {
            goblinTexture = goblinTex;
            rabbitTexture = rabbitTex;
        }
        public void AddType(EnemyType type)
        {
            enemyTypes.Add(type);
        }
        public void Update(TileMap map, Hero hero, List<Enemy> enemies)
        {
            if (!hasSpawned)
            {
                SetDifficulty();
                SpawnEnemies(map, hero, enemies);
                hasSpawned = true;
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

                if (Vector2.Distance(pos, hero.Position) < Rad)
                    continue;

                Enemy enemy = CreateEnemy(pos);
                enemies.Add(enemy);
                spawned++;
            }
        }
        public void ForceRespawn()
        {
            hasSpawned = false;
            respawnTimer = 0f;
        }
        public void OnNightStarted()
        {
            MaxEnemies = baseMaxEnemies * NightMultiplier;
            ForceRespawn();
        }

        public void OnDayStarted()
        {
            MaxEnemies = baseMaxEnemies;
            ForceRespawn();
        }
        private Enemy CreateEnemy(Vector2 position)
        {

            EnemyType type = enemyTypes[random.Next(enemyTypes.Count)];

            switch (type)
            {
                case EnemyType.Goblin:
                    return new Goblins(goblinTexture, position);

                case EnemyType.Rabbit:
                    return new EvilRabbit(rabbitTexture, position);

                default:
                    return new Goblins(goblinTexture, position);
            }
        }
    }
}
