using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Recource_Collection
{
    public class Hero : Sprite
    {


        private const float SPEED = 500;

        public Vector2 Velocity { get; set; }
        public int MaxHealth { get; set; } = 100;
        public int CurrentHealth { get;  set; }

        public Rectangle HitBox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        public Rectangle AttackHitbox { get; private set; }
        public bool IsAttacking { get; set; }


        private bool attackHasHit = false;
        public int damageTimer;
        public int damageCooldown = 60;
        public int heroDamage = 10;
        public int attackRadius = 200;


        public Hero(int maxhealth, Texture2D texture, Vector2 position) : base(texture, position)
        {
            MaxHealth = maxhealth;
            Texture = texture;
            Position = position;
            CurrentHealth = maxhealth;
        }
        

        public void DealDamage(Boss enemy)
        {
            if (IsAttacking && !attackHasHit && AttackHitbox.Intersects(enemy.Hitbox))
            {
                enemy.CurrentHealth -= heroDamage;
                attackHasHit = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                attackHasHit = false;
        }

        public void TakeDamage(int damageDealt)
        {
            if (damageTimer >= damageCooldown)
            {
                CurrentHealth -= damageDealt;
                damageTimer = 0; 
            }
        }


        public void Heal(int healAmount)
        {
            CurrentHealth = Math.Min(CurrentHealth + healAmount, MaxHealth);
        }




        public void Update()
        {
            Velocity = SPEED * InputManager.Direction;
            var keyboardState = Keyboard.GetState();
            Position += new Vector2(Velocity.X, Velocity.Y) * Globals.Time;
            IsAttacking = false;
            damageTimer++;

           
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                IsAttacking = true;
                Vector2 topLeft = Position - Origin;

                int attackX = (int)(topLeft.X - (attackRadius - Texture.Width) / 2f);
                int attackY = (int)(topLeft.Y - (attackRadius - Texture.Height) / 2f);

                AttackHitbox = new Rectangle(attackX, attackY, attackRadius, attackRadius);

            }
        }


        public void Draw()
        {

            Globals.SpriteBatch.Draw(Texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);

        }
    }
}

