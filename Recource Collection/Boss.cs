using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    public class Boss
    {
        public int Health { get; set; }
        public Texture2D SpriteSheet { get; private set; }
        public Vector2 Position { get; set; }
        public AnimationManager AnimationManager { get; private set; }

        public Boss(Texture2D spriteSheet, Vector2 position, int health)
        {
            Health = health;
            SpriteSheet = spriteSheet;
            Position = position;

            var bossAnimations = new Dictionary<AnimationManager.AnimationName, Animation>
            {
                { AnimationManager.AnimationName.idle, new Animation(2, 30, 1, new Vector2(128, 128)) },
                { AnimationManager.AnimationName.smashAttack, new Animation(8, 10, 0, new Vector2(128, 128)) } 
            };

            AnimationManager = new AnimationManager(bossAnimations);
        }

        public void Update()
        {
            AnimationManager.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = AnimationManager.GetSourceRect(32, 32);
            Vector2 size = AnimationManager.GetSize(); 
            Rectangle destRectangle = new Rectangle((int)Position.X,(int)Position.Y,(int)size.X,(int)size.Y);
            spriteBatch.Draw(SpriteSheet, destRectangle, sourceRectangle, Color.White);
        }
    }
}
