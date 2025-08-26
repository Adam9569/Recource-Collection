using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;



namespace Recource_Collection
{
    public class Hero : Sprite
    {
        
       
        private const float SPEED = 500;

        public Vector2 Velocity { get; set; }

        public Rectangle HitBox { get; private set; }
        public int Weight { get; set; }
        public int MaxWeight = 100;
        public Dictionary<Items, int> Inventory { get; set; } = new Dictionary<Items, int>(); 


        public Hero(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Texture = texture;
            Position = position;
            HitBox = new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
        }





        public void Update()
        {
            var keyboardState = Keyboard.GetState();
            Velocity = SPEED * InputManager.Direction;
            Position += new Vector2(Velocity.X, Velocity.Y) * Globals.Time;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);


        }


        public void Draw()
        {
            
            Globals.SpriteBatch.Draw(Texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
          
            
        }
    }
}

