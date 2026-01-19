using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Recource_Collection.Scenes;



namespace Recource_Collection
{
    public class Hero : Sprite
    {
 
        public Rectangle HitBox { get; private set; }
        public Rectangle AttackHitBox { get; private set; }
        public int QuestionsCorrect { get; set; }
        public int QuestionsIncorrect { get; set; }

        public bool IsAttacking { get; set; }
        public int heroDamage = 10;

        public int Weight { get; set; }
        public int CurrentHunger;
        public int CurrentThirst;

        public int MaxWeight = 100;     
        public int MaxHunger = 100;
        public int HungerCounter = 0;
        public int MaxThirst = 100;
        public int ThirstCounter = 0;

        private bool attackHasHit = false;
        public int damageTimer;
        public int damageCooldown = 60;
        public int attackRadius = 200;

        public int MaxHealth { get; set; } = 100;
        public int CurrentHealth { get; private set; }

        public bool Isfarming { get; set; }
        List<string> FarmingTiles = new List<string>();

        private KeyboardState previousState;
        private string farmTileKey;
        private float farmProgress;
        private float timeNeeded;
        private MainMenuScene menu;


        private float SPEED = 500;
        public int MaxStamina { get; set; }
        public int CurrentStamina { get; set; }

        public int sprintTimer = 0;
        public int sprintCooldown = 45;
        public int recoveryTimer = 0;
        public int recoveryCooldown = 30;
        public bool staminaUsed = false;
        public int staminaUsedTimer = 0;

        public int HealthBarSub;
        public int HungerBarSub;
        public int ThirstBarSub;
        public int BarHeight = 30;
        public Rectangle HealthBar;
        public Rectangle HungerBar;
        public Rectangle ThirstBar;

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

        public Dictionary<Items, int> Inventory { get; set; } = new Dictionary<Items, int>();

        public Hero(int maxstamina,int MaxHealth, Texture2D texture, Vector2 position) : base(texture, position)
        {
            HitBox = new Rectangle((int)position.X - Texture.Width /2 , (int)position.Y - Texture.Height /2 , Texture.Width, Texture.Height);
            AttackHitBox = new Rectangle((int)position.X - Texture.Width / 2, (int)position.Y - Texture.Height / 2, Texture.Width * (int)1.5, Texture.Height * (int)1.5);
            SPEED = 500;
            CurrentHunger = MaxHunger;
            CurrentThirst = MaxThirst;
            CurrentHealth = MaxHealth;
            MaxStamina = maxstamina;
            CurrentStamina = MaxStamina;
        }

        public void addToInv(Items itemtype)
        { 
            if (Weight + CollectableItems.Weight[itemtype] <= MaxWeight)
            {
                if (Inventory.ContainsKey(itemtype))
                {
                    Inventory[itemtype]++;
                }
                else
                {
                    Inventory[itemtype] = 1;
                }
                Weight += CollectableItems.Weight[itemtype];
            }
        }

        public void Recovery()
        {

            if (staminaUsed == false && CurrentStamina != MaxStamina)
            {
                staminaUsedTimer++;
                if (staminaUsedTimer >= 60)
                {
                    CurrentStamina += 10;
                    staminaUsedTimer = 0;
                }
            }
        }

        public void Sprint(KeyboardState keyboardState)
        {

            if (SPEED <= 1000 && keyboardState.IsKeyDown(Keys.LeftShift) && CurrentStamina > StaminaVal[Actions.sprint])
            {
                staminaUsed = true;
                SPEED += 2;
                if (sprintTimer >= sprintCooldown)
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

        public void TakeDamage(int damageDealt)
        {
            if (damageTimer > 0) return;

            CurrentHealth -= damageDealt;
            damageTimer = damageCooldown;
        }
        public void DealDamage(Enemy enemy)
        {
            if (IsAttacking && !attackHasHit && AttackHitBox.Intersects(enemy.enemyHitBox))
            {
                enemy.CurrentHealth -= heroDamage;
                attackHasHit = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                attackHasHit = false;
            }   
                
        }
        public void DealBossDamage(Boss boss)
        {
            if (IsAttacking && !attackHasHit && AttackHitBox.Intersects(boss.Hitbox))
            {
                boss.CurrentHealth -= heroDamage;
                attackHasHit = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                attackHasHit = false;
            }
        }

        public void Heal(int healAmount)
        {
            CurrentHealth = Math.Min(CurrentHealth + healAmount, MaxHealth);
        }

        public void PassiveNeeds()
        {
            if (CurrentHunger != 0)
            {
                HungerCounter++;
                if(HungerCounter > 2700)
                {
                    CurrentHunger--;
                    HungerCounter = 0;
                }
            }

            if (CurrentThirst != 0)
            {
                ThirstCounter++;
                if (ThirstCounter > 1800)
                {
                    CurrentThirst--;
                    ThirstCounter = 0;
                }
            }
        }


        public void Debuffs()
        {
            if(CurrentThirst < 25 || CurrentHunger < 25)
            {
                SPEED = 250;
            }
        }
        public void Eating(Items itemtype)
        {
            Weight -= CollectableItems.Weight[itemtype];
            CurrentHunger = CurrentHunger + Math.Min(CollectableItems.Food[itemtype],MaxHunger);
            Inventory[itemtype]--;
        }
        public void Drinking(Items itemtype)
        {
            Weight -= CollectableItems.Weight[itemtype];
            CurrentThirst = Math.Min(CollectableItems.Drink[itemtype],MaxThirst);
            Inventory[itemtype]--;
        }

        public void UpdateEssentialBars()
        {
            int barX = (int)Position.X + Globals.WindowSize.X / 2 - 120;
            int barY = (int)Position.Y - Globals.WindowSize.Y / 2 + 300;

            HealthBar = new Rectangle(barX, barY, (int)(float)(CurrentHealth / MaxHealth) * 100, BarHeight);

            HungerBar = new Rectangle(barX, barY - 40, CurrentHunger * 2, BarHeight);

            ThirstBar = new Rectangle(barX, barY - 80, CurrentThirst * 2, BarHeight);
        }



        public void removeFromInv(Items itemtype)
        {
            if (Inventory.ContainsKey(itemtype) && Inventory[itemtype] >= 1)
            {
                Weight -= CollectableItems.Weight[itemtype];
                Inventory[itemtype]--;
            }
        }
        private string TileKeyFromPoint(TileMap map, int px, int py)
        {
            int tx = px / TileMap.tilesize;
            int ty = py / TileMap.tilesize;
            return $"{tx};{ty}";
        }
        private List<string> CornerTiles(TileMap map)
        {
            string tl = TileKeyFromPoint(map, HitBox.Left, HitBox.Top);
            string tr = TileKeyFromPoint(map, HitBox.Right - 1, HitBox.Top);
            string bl = TileKeyFromPoint(map, HitBox.Left, HitBox.Bottom - 1);
            string br = TileKeyFromPoint(map, HitBox.Right - 1, HitBox.Bottom - 1);
            var set = new HashSet<string> { tl, tr, bl, br };
            foreach (var k in set.ToList())
            {
                if (!map.InTileMap(k))
                    set.Remove(k);
            }

            return new List<string>(set);
        }
        public void Death()
        {
            if (CurrentHealth <= 0)
            {
                Globals.QuitGame();
            }
        }
        private bool Farming(TileMap map)
        {
            foreach (var key in CornerTiles(map))
            {
                var t = map.GetTile(key);

                if (map.FarmTime.TryGetValue(t, out float requiredTime))
                {
                    Isfarming = true;
                    farmTileKey = key;
                    timeNeeded = requiredTime;
                    farmProgress = 0f;
                    return true;
                }
            }
            return false;
        }
        public void SetStats(int health, int hunger, int thirst)
        {
            CurrentHealth = Math.Clamp(health, 0, MaxHealth);
            CurrentHunger = Math.Clamp(hunger, 0, MaxHunger);
            CurrentThirst = Math.Clamp(thirst, 0, MaxThirst);
        }
        public void SetPosition(Vector2 pos)
        {
            Position = pos;
            HitBox = new Rectangle((int)Position.X - Texture.Width / 2,(int)Position.Y - Texture.Height / 2,Texture.Width,Texture.Height);
        }
        public void Update(TileMap map)
        {
            var keyboardState = Keyboard.GetState();
            bool fDown = keyboardState.IsKeyDown(Keys.F);
            bool fPressed = fDown && !previousState.IsKeyDown(Keys.F);
            UpdateEssentialBars();
            Sprint(keyboardState);

            if (CurrentStamina < MaxStamina)
            {
                Recovery();

            }
            else
            {
                CurrentStamina = MaxStamina;
            }


            IsAttacking = false;
            if (damageTimer > 0)
                damageTimer--;
            if(keyboardState.IsKeyDown(Keys.H) && !previousState.IsKeyDown(Keys.H) && CurrentHealth != MaxHealth)
            {
                CurrentHealth = CurrentHealth + 2;
            }
            if(CurrentHealth <= 0)
            {
                Inventory.Clear();
                Position = new Vector2(100,100);
                CurrentHunger = 50;
                CurrentThirst = 50;
                CurrentHealth = MaxHealth;
            }

            if (Isfarming)
            {
                Velocity = Vector2.Zero;
                farmProgress += (float)Globals.Time;

                if (farmProgress >= timeNeeded)
                {
                    var t = map.GetTile(farmTileKey);
                    if (map.FarmDrops.TryGetValue(t, out Items drop))
                    {
                        int amount = 1;
                        map.DropAmount.TryGetValue(t, out amount);

                        for (int i = 0; i < amount; i++)
                            addToInv(drop);

                        map.SetTile(farmTileKey, TileMap.TileType.grass);
                    }
                    Isfarming = false;
                    farmProgress = 0f;
                    timeNeeded = 0f;
                    farmTileKey = null;
                }
                HitBox = new Rectangle((int)Position.X - Texture.Width / 2,(int)Position.Y - Texture.Height / 2,Texture.Width, Texture.Height);
                Debuffs();
                PassiveNeeds();
                previousState = keyboardState;
                return;
            }
            Velocity = SPEED * InputManager.Direction;
            Position += Velocity * (float)Globals.Time;

            HitBox = new Rectangle((int)Position.X - Texture.Width / 2,(int)Position.Y - Texture.Height / 2,Texture.Width, Texture.Height);

            if (fPressed)
            {
                Farming(map);
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                IsAttacking = true;
                Vector2 topLeft = Position - Origin;

                int attackX = (int)(topLeft.X - (attackRadius - Texture.Width) / 2f);
                int attackY = (int)(topLeft.Y - (attackRadius - Texture.Height) / 2f);

                AttackHitBox = new Rectangle(attackX, attackY, attackRadius, attackRadius);

            }

            Debuffs();
            PassiveNeeds();
            previousState = keyboardState;
        }

    }
}

