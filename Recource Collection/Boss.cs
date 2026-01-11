using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;

namespace Recource_Collection
{
    public class Boss
    {
        public int CurrentHealth { get; set; }
        public int Health = 500;
        public Texture2D SpriteSheet { get; private set; }
        public Vector2 Position { get; set; }
        public AnimationManager AnimationManager { get; private set; }

        
        public int Damage = 25;

        int sCounter;
        int smashCooldown = 600;
        int hCounter;
        int hCooldown = 300;
        public BossProjectiles _bossProjectiles;
        public List<BossProjectiles> Projectiles = new List<BossProjectiles>();
        public Texture2D projectileTexture;
       
        


        public Boss(int Maxhealth, Texture2D spriteSheet, Vector2 position)
        {
            CurrentHealth = Maxhealth;
            SpriteSheet = spriteSheet;
            Position = position;
            sCounter = 0;

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
            Projectiles.Clear();

            for (int i = 1; i < 30; i++)
            {
                Projectiles.Add(new BossProjectiles(projectileTexture,Position));
            }
            
        }
        public void Update(Hero hero)
        {

            hCounter++;
            sCounter++;
            if(hCounter == hCooldown)
            {
                FireDiagonal();
            }
            if (sCounter >= smashCooldown )
            {
                AnimationManager.Play(AnimationManager.BossAnimations.smashAttack);
                FireSpiral();
                sCounter = 0;
            }
            else if (AnimationManager.current == AnimationManager.BossAnimations.smashAttack && AnimationManager.finishedAnimation)
            {
                AnimationManager.Play(AnimationManager.BossAnimations.idle);
            }

            AnimationManager.Update();
            foreach (BossProjectiles projectile in Projectiles)
            {
                projectile.Update();


                if (projectile.HitBox.Intersects(hero.HitBox))
                {
                    hero.TakeDamage(projectile.Damage);
                    projectile.Deactivate();
                }
            }
           
            
        }

        public void FireSpiral()
        {
            Vector2 center = Position;

            float startRadius = 500f;
            float horizontalSpeed = 120f;
            float angularSpeed = 6f;
            int pCount = 9;

            for (int i = 0; i < pCount; i++)
            {
                float angle = MathHelper.TwoPi * (i / (float)Projectiles.Count);
                float r = startRadius + i * 6f;

                Projectiles[i].SpawnSpiral(center, r, angle, angularSpeed, horizontalSpeed);
            }
        }

        public void FireDiagonal()
        {
            Vector2 velocity = new Vector2(350f, 350f);
            int meteors = 6;

            for (int i = 0; i < meteors; i++)
            {
                BossProjectiles projectile = Projectiles.Find(x => !x.Active);
                if (projectile == null) break;

                float y = 40f + i * 50f;
                Vector2 start = new Vector2(-100f, y);

                projectile.SpawnMeteor(start, velocity);
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
                if (!projectile.Active) continue;

                spriteBatch.Draw( projectileTexture, projectile.Position, null, Color.White, 0f, new Vector2(projectileTexture.Width / 2f, projectileTexture.Height / 2f), 0.1f, SpriteEffects.None, 1f);
            }



        }
    }
}
