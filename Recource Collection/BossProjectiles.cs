using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    public enum ProjectilePattern
    {
        Spiral,
        Diagonal
    }

    public class BossProjectiles : Projectiles
    {
        public ProjectilePattern Pattern { get; set; }

        public float Radius;
        public float Angle;
        public float AngularSpeed;
        public float RadiusSpeed;
        public Vector2 Center;

        public Vector2 Velocity;
        public Vector2 Start;
        public Vector2 End;

        public bool Active { get; private set; } = false;
        public float LifeTime { get; private set; } = 0f;
        public float MaxLifeSpan { get; set; } = 12f;

        public Rectangle HitBox
        {
            get
            {
                int size = 32;
                return new Rectangle((int)Position.X - size / 2,(int)Position.Y - size / 2,size,size);
            }
        }

        public BossProjectiles(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Damage = 15;
        }

        public void SpawnSpiral(Vector2 center, float startRadius, float startAngle, float angularSpeed, float radiusSpeed)
        {
            Pattern = ProjectilePattern.Spiral;
            Center = center;
            Radius = startRadius;
            Angle = startAngle;
            AngularSpeed = angularSpeed;
            RadiusSpeed = radiusSpeed;

            Active = true;
            LifeTime = 0f;
            Position = Center + new Vector2((float)(Radius * Math.Cos(Angle)), (float)(Radius * Math.Sin(Angle)));
        }

        public void SpawnMeteor(Vector2 start, Vector2 velocity)
        {
            Pattern = ProjectilePattern.Diagonal;
            Start = start;
            Velocity = velocity;

            Active = true;
            LifeTime = 0f;

            Position = Start;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public void Update()
        {
            if (!Active) return;

            LifeTime += Globals.Time;
            if (LifeTime >= MaxLifeSpan)
            {
                Active = false;
                return;
            }

            switch (Pattern)
            {
                case ProjectilePattern.Spiral:
                    Angle += AngularSpeed * Globals.Time;
                    Radius -= RadiusSpeed * Globals.Time; 

                    if (Radius <= 0f)
                    {
                        Active = false;
                        return;
                    }

                    Position = Center + new Vector2((float)(Radius * Math.Cos(Angle)),(float)(Radius * Math.Sin(Angle)));
                    break;

                case ProjectilePattern.Diagonal:
                    Position += Velocity * Globals.Time;
                    if (Position.X > Globals.WindowSize.X + 200 || Position.Y > Globals.WindowSize.Y + 200)
                    {
                        Active = false;
                    }
                    break;
            }
        }
    }
}
