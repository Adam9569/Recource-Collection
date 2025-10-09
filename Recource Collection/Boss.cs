using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
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
        //public BossProjectiles _bossProjectiles = new BossProjectiles();
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
                BossProjectiles _BossProjectile = new BossProjectiles(projectileTexture, _bossProjectiles.Origin);
                Projectiles.Add(_BossProjectile);
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
            //_bossProjectiles.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = AnimationManager.GetSourceRect(33, 33);
            Vector2 size = AnimationManager.GetSize(); 
            Rectangle destRectangle = new Rectangle((int)(Position.X - size.X /2 ),(int)(Position.Y - size.Y /2),(int)size.X,(int)size.Y);
            spriteBatch.Draw(SpriteSheet, destRectangle, sourceRectangle, Color.White);


        }
    }
}
