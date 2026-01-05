using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Recource_Collection
{
    public class Enemy : Sprite
    {
        public float AggroRangePixels = TileMap.tilesize * 12f;
        public bool IsAggro { get; private set; }

        private int pathCounter;
        private List<string> Path = new List<string>();
        private int Counter = 0;
        private int damage = 10;

        public Enemy(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Scale = 2f;
            Speed = 20;
        }

        public void Update(TileMap tileMap, Hero hero)
        {
            float dist = Vector2.Distance(Position, hero.Position);
            IsAggro = dist <= AggroRangePixels;

            if (!IsAggro)
            {
                Path.Clear();
                Velocity = Vector2.Zero;
                pathCounter = 0;
                return;
            }
            PathTimer(tileMap, hero);
            Move(tileMap, 200f);
            pathCounter++;
        }

        public void PathTimer(TileMap tileMap, Hero hero)
        {
            if (pathCounter > 60)
            {
                Counter = 0;
                Path = Pathfind(tileMap, hero);
                pathCounter = 0;
                Debug.WriteLine($"Counter : {Counter}");
            }   
            if (Path == null || Path.Count == 0)
            {
                Counter = 0;
                Path = Pathfind(tileMap, hero);
            }
        }
        public void InputDamage(Hero hero)
        {
            
        }

        public List<string> Pathfind(TileMap tileMap, Hero hero)
        {
            List<string> path = new List<string>();
            Dictionary<string, string> visited = new Dictionary<string, string>();
            Queue<string> unvisited = new Queue<string>();

            string target = tileMap.VectorToPosition(hero.Position);
            string start = tileMap.VectorToPosition(Position);

            if (!tileMap.InTileMap(start) || !tileMap.InTileMap(target))
                return path;

            unvisited.Enqueue(start);
            visited[start] = null;

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
                    if (!visited.ContainsKey(neighbour))
                    {
                        visited[neighbour] = currentTile;
                        unvisited.Enqueue(neighbour);
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

        public void draw(Texture2D evilTexture)
        {
            foreach (string point in Path)
            {
                string[] coords = point.Split(';');
                Globals.SpriteBatch.Draw(
                    evilTexture,
                    new Rectangle(int.Parse(coords[0]) * 128, int.Parse(coords[1]) * 128, 30, 30),
                    Color.Red
                );
            }
        }
    }
}
