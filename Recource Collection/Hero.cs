using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;

namespace Recource_Collection
{
    public class Hero : Sprite
    {


        private float SPEED = 500;

        public Vector2 Velocity { get; set; }
        public int MaxHealth { get; set; } = 100;
        public int CurrentHealth { get;  set; }
        public int MaxStamina { get; set; }
        public int CurrentStamina { get; set; }

        public Rectangle HitBox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        public Rectangle AttackHitbox { get; private set; }
        public bool IsAttacking { get; set; }


        public int damageTimer;
        public int damageCooldown = 60;
        public int sprintTimer = 0;
        public int sprintCooldown = 45;
        public int recoveryTimer = 0;
        public int recoveryCooldown = 30;
        public int heroDamage = 10;
        public int attackRadius = 200;
        public bool staminaUsed = false;
        public int staminaUsedTimer = 0;

        public enum Actions
        {
            hit,
            swing,
            sprint
        }
        public static Dictionary<Actions, int> StaminaVal = new Dictionary<Actions, int>()
        {
            {Actions.hit,4 },
            {Actions.swing,8},
            {Actions.sprint,6}
        };


        public Hero(int maxstamina,int maxhealth, Texture2D texture, Vector2 position) : base(texture, position)
        {
            MaxHealth = maxhealth;
            Texture = texture;
            Position = position;
            CurrentHealth = maxhealth;
            MaxStamina = maxstamina;
            CurrentStamina = MaxStamina;
            
        }
        


        public void TakeDamage(int damageDealt)
        {
            if (damageTimer >= damageCooldown)
            {
                CurrentHealth -= damageDealt;
                damageTimer = 0; 
            }
        }
        
        public void Recovery()
        {
            
            if(staminaUsed == false && CurrentStamina != MaxStamina)
            {
                staminaUsedTimer++;
                if(staminaUsedTimer >= 60)
                {
                    CurrentStamina += 10;
                    staminaUsedTimer = 0;
                }
            }
        }

        public void Sprint(KeyboardState keyboardState)
        {

            if(SPEED <= 1200 && keyboardState.IsKeyDown(Keys.LeftShift) && CurrentStamina > StaminaVal[Actions.sprint])
            {
                staminaUsed = true;
                SPEED+=2;
                if(sprintTimer >= sprintCooldown)
                {
                    CurrentStamina -= StaminaVal[Actions.sprint];
                    sprintTimer = 0;
                }
            }
            else
            {
                SPEED = 500;
                staminaUsed = false;
            }
            
        }


        public void Heal(int healAmount)
        {
            CurrentHealth = Math.Min(CurrentHealth + healAmount, MaxHealth);
        }




        public void Update()
        {
            sprintTimer++;
            Velocity = SPEED * InputManager.Direction;
            var keyboardState = Keyboard.GetState();
            Position += new Vector2(Velocity.X, Velocity.Y) * Globals.Time;
            IsAttacking = false;
            Sprint(keyboardState);

            if(CurrentStamina < MaxStamina)
            {
                Recovery();

            }
            else
            {
                CurrentStamina = MaxStamina;
            }
            

           
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

