using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    public class BossProjectiles : Projectiles
    {
        public float Distance = 0;
        public float Angle = 0;
        

        public BossProjectiles(Texture2D texture, Vector2 position ) : base (texture,position)
        {
            Damage = 15;
        }

        public void ResetPosition()
        {
            if (Globals.WindowSize.X < Origin.X)
            {
                Position = Origin;
            }
        }
        public void Movement()
        {
            Position = new Vector2((float)(Distance * Math.Cos(Angle)), (float)(Distance * Math.Sin(Angle)));

        }

        public void Update()
        {
            Distance = Distance + (float)0.1;
            Angle = Angle + (float)0.1;
            Movement();
        }


        
    }
}
