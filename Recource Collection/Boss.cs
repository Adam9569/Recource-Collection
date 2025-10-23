using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    public class Boss
    {
        public int Health { get; set; }
        public Texture2D SpriteSheet { get; private set; }
        public Vector2 Position { get; set; }
        public AnimationManager AnimationManager { get; private set; }

        
        public int Damage = 25;

        int counter;
        int smashCooldown = 240;
        public BossProjectiles _bossProjectiles;
        public List<BossProjectiles> Projectiles = new List<BossProjectiles>();
        public Texture2D projectileTexture;
       
        


        public Boss(Texture2D spriteSheet, Vector2 position, int health)
        {
            Health = health;
            SpriteSheet = spriteSheet;
            Position = position;
            counter = 0;

            var bossAnimations = new Dictionary<AnimationManager.BossAnimations, Animation>
            {
                { AnimationManager.BossAnimations.idle, new Animation(2, 30, 1, new Vector2(128, 128)) },
                { AnimationManager.BossAnimations.smashAttack, new Animation(8, 20, 0, new Vector2(256,256)) } 
            };

            AnimationManager = new AnimationManager(bossAnimations);
        }
        public Rectangle Hitbox
        {
            get
            { 
                Vector2 size = AnimationManager.GetSize();
                return new Rectangle((int)Position.X,(int)Position.Y,(int)size.X,(int)size.Y);
            }
        }

        public void LoadContent(ContentManager Content)
        {
            projectileTexture = Content.Load<Texture2D>("WoodenSword");

            for (int i = 1; i < 10; i++)
            {
                _bossProjectiles = new BossProjectiles(projectileTexture, new Vector2(0, 0));
                Projectiles.Add(_bossProjectiles);
            }
            
        }
        public void Update()
        {
           

            counter++;
            if (counter >= smashCooldown)
            {
                AnimationManager.Play(AnimationManager.BossAnimations.smashAttack);
                counter = 0;
            }
            else if (AnimationManager.current == AnimationManager.BossAnimations.smashAttack && AnimationManager.finishedAnimation)
            {
                AnimationManager.Play(AnimationManager.BossAnimations.idle);
            }

            AnimationManager.Update();
            foreach (BossProjectiles projectile in Projectiles)
            {
                projectile.Update();
            }
           
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = AnimationManager.GetSourceRect(33, 33);
            Vector2 size = AnimationManager.GetSize(); 
            Rectangle destRectangle = new Rectangle((int)(Position.X - size.X /2 ),(int)(Position.Y - size.Y /2),(int)size.X,(int)size.Y);
            spriteBatch.Draw(SpriteSheet, destRectangle, sourceRectangle, Color.White);
            foreach (BossProjectiles projectile in Projectiles)
            {
                float rotation = 0f;
                spriteBatch.Draw(projectileTexture,
                    new Rectangle((int)projectile.Position.X, (int)projectile.Position.Y, projectileTexture.Width, projectileTexture.Height), 
                    null, 
                    Color.White, 
                    rotation, 
                    new Vector2(projectileTexture.Width / 2, projectileTexture.Height / 2), 
                    SpriteEffects.None, 
                    1.0f);
            }



        }
    }
}
