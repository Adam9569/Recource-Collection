using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

namespace Recource_Collection
{
    public class Enemy : Sprite
    {
        public float AggroRange { get; set; }
        public bool IsAggro { get; private set; }

        private int pathCounter;
        private List<string> Path = new List<string>();
        private int Counter = 0;
        private int DamageTimer = 0;

        public int MaxHealth { get; set; } 
        public int Damage { get; set; }
        public int CurrentHealth { get; set; }
        public Rectangle enemyHitBox { get; set; }
        private int DamageCooldown = 60;



        public Enemy(int maxHealth, int damage, float aggroRange, Texture2D texture, Vector2 position)
    : base(texture, position)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            Damage = damage;
            AggroRange = aggroRange;
        }

        public void Update(TileMap tileMap, Hero hero)
        {
            float dist = Vector2.Distance(Position, hero.Position);
            enemyHitBox = new Rectangle((int)Position.X - Texture.Width / 2, (int)Position.Y - Texture.Height / 2, Texture.Width, Texture.Height);
            IsAggro = dist <= AggroRange;

            if(enemyHitBox.Intersects(hero.HitBox))
            {
                DamageTimer++;
            }
            else
                DamageTimer = 0;


            if(DamageTimer >= DamageCooldown)
            {
                hero.TakeDamage(Damage);
                DamageTimer = 0;
            }

            if (!IsAggro)
            {
                Path.Clear();
                Velocity = Vector2.Zero;
                pathCounter = 0;
                return;
            }
            PathTimer(tileMap, hero);
            Move(tileMap, 300f);
            pathCounter++;
        }

        public void PathTimer(TileMap tileMap, Hero hero)
        {
            if (pathCounter > 60)
            {
                Counter = 0;
                Path = Pathfind(tileMap, hero);
                pathCounter = 0;
            }   
            if (Path == null || Path.Count == 0)
            {
                Counter = 0;
                Path = Pathfind(tileMap, hero);
            }
        }
        float Heuristic(string a, string b)
        {
            int[] A = Array.ConvertAll(a.Split(';'), int.Parse);
            int[] B = Array.ConvertAll(b.Split(';'), int.Parse);

            return MathF.Abs(A[0] - B[0]) + MathF.Abs(A[1] - B[1]);
        }

        public List<string> Pathfind(TileMap tileMap, Hero hero)
        {
            List<string> path = new List<string>();
            Dictionary<string, string> visited = new Dictionary<string, string>();
            Dictionary<string, float> distanceFromStart = new Dictionary<string, float>();
            PriorityQueue<string, float> unvisited = new PriorityQueue<string, float>();

            string target = tileMap.VectorToPosition(hero.Position);
            string start = tileMap.VectorToPosition(Position);

            if (!tileMap.InTileMap(start) || !tileMap.InTileMap(target)) return path;

            visited[start] = null;
            distanceFromStart[start] = 0;
            unvisited.Enqueue(start, 0);

            bool targetFound = false;

            while (unvisited.Count > 0)
            {
                Counter++;
                string currentTile = unvisited.Dequeue();

                if (currentTile == target)
                {
                    targetFound = true;
                    break;
                }

                foreach (string neighbour in tileMap.GetNeighbours(currentTile, true))
                {
                    float newDistance = distanceFromStart[currentTile] + 1;

                    if (!distanceFromStart.ContainsKey(neighbour) || newDistance < distanceFromStart[neighbour])
                    {
                        visited[neighbour] = currentTile;
                        distanceFromStart[neighbour] = newDistance;

                        float priority = newDistance + Heuristic(neighbour, target);
                        unvisited.Enqueue(neighbour, priority);
                    }
                }
            }

            if (targetFound)
            {
                string currentTile = target;
                while (currentTile != null)
                {
                    path.Add(currentTile);
                    currentTile = visited[currentTile];
                }
                path.Reverse();
            }

            return path;
        }

        public void Move(TileMap tilemap, float speed)
        {
            if (Path == null || Path.Count <= 0) return;

            string currentTile = tilemap.VectorToPosition(Position);
            if (Path.Count > 0 && currentTile == Path[0])
            {
                Path.RemoveAt(0);
            }

            if (Path.Count <= 0) return;

            string nextTile = Path[0];
            Vector2 nextPosition = tilemap.PositionToVector(nextTile);

            Vector2 direction = nextPosition - Position;
            if (direction.Length() > 0)
            {
                direction = Vector2.Normalize(direction);
                Velocity = speed * direction;
                Position += Velocity * Globals.Time;
            }

            if (Vector2.Distance(Position, nextPosition) < 2f)
            {
                Path.RemoveAt(0);
            }
        }

        public void draw(Texture2D evilTexture){}
    }
}
