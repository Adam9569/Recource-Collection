using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;



namespace health_management
{
    public class Hero : Sprite
    {
        
       
        private const float SPEED = 500;

        public Vector2 Velocity { get; set; }
        public int MaxHealth { get; set; } = 100;
        public int CurrentHealth { get; private set; }

        public Rectangle HitBox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);



        public Hero(int maxhealth ,Texture2D texture, Vector2 position) : base(texture, position)
        {
            MaxHealth = maxhealth;
            Texture = texture;
            Position = position;
            CurrentHealth = maxhealth;
        }

        public void TakeDamage(int damageDealt)
        {
            CurrentHealth = Math.Max(CurrentHealth - damageDealt,0);
        }
        
        public void Heal(int healAmount)
        {
            CurrentHealth = Math.Min(CurrentHealth + healAmount,MaxHealth);
        }




        public void Update()
        {
            Velocity = SPEED * InputManager.Direction;
            Position += new Vector2(Velocity.X, Velocity.Y) * Globals.Time;
            

           

        }


        public void Draw()
        {
            
            Globals.SpriteBatch.Draw(Texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
            
        }
    }
}

