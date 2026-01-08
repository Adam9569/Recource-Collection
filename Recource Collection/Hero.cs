using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.Linq;



namespace Recource_Collection
{
    public class Hero : Sprite
    {
 
        public Rectangle HitBox { get; private set; }
        public Rectangle AttackHitBox { get; private set; }
        public bool IsAttacking { get; set; }
        public int heroDamage { get; set; }

        public int Weight { get; set; }
        public int CurrentHunger = 50;
        public int CurrentThirst = 50;

        public int MaxWeight = 100;     
        public int MaxHunger = 100;
        public int HungerCounter = 0;
        public int MaxThirst = 100;
        public int ThirstCounter = 0;

        public int attackRadius = 200;
        public int damageTimer;
        private int AttackTime = 0;
        private bool attackHit = false;

        public int MaxHealth { get; set; } = 100;
        public int CurrentHealth { get; private set; }

        public bool Isfarming { get; set; }
        List<string> FarmingTiles = new List<string>();

        private KeyboardState previousState;
        private string farmTileKey;
        private float farmProgress;
        private float timeNeeded;


        public Dictionary<Items, int> Inventory { get; set; } = new Dictionary<Items, int>();

        public Hero(int MaxHealth, Texture2D texture, Vector2 position) : base(texture, position)
        {
            HitBox = new Rectangle((int)position.X - Texture.Width /2 , (int)position.Y - Texture.Height /2 , Texture.Width, Texture.Height);
            AttackHitBox = new Rectangle((int)position.X - Texture.Width / 2, (int)position.Y - Texture.Height / 2, Texture.Width * (int)1.5, Texture.Height * (int)1.5);
            Speed = 500;
            CurrentHealth = MaxHealth;
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
        public void TakeDamage(int damageDealt)
        {
            CurrentHealth = Math.Max(CurrentHealth - damageDealt, 0);
        }
        
        public void Heal(int healAmount)
        {
            CurrentHealth = Math.Min(CurrentHealth + healAmount, MaxHealth);
        }
        private void StartAttack()
        {
            IsAttacking = true;
            AttackTime = 0;
            attackHit = false;

            Vector2 topLeft = Position - Origin;
            int attackX = (int)(topLeft.X - (attackRadius - Texture.Width) / 2f);
            int attackY = (int)(topLeft.Y - (attackRadius - Texture.Height) / 2f);
            AttackHitBox = new Rectangle(attackX, attackY, attackRadius, attackRadius);
        }
        private void Attack(KeyboardState keyboardState)
        {
            if (damageTimer > 0)
                damageTimer--;

            bool spaceDown = keyboardState.IsKeyDown(Keys.Space);
            bool spacePressed = spaceDown && !previousState.IsKeyDown(Keys.Space);

            if (keyboardState.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space) && damageTimer == 0)
            {
                StartAttack();
            }

            if (IsAttacking)
            {
                AttackTime++;

                if (!spaceDown)
                {
                    IsAttacking = false;
                    AttackHitBox = Rectangle.Empty;
                    damageTimer = 15;
                }
            }
        }

        public void DealDamage(Enemy enemy)
        {
            if (!IsAttacking) return;
            if (attackHit) return;

            if (AttackHitBox.Intersects(enemy.enemyHitBox))
            {
                enemy.CurrentHealth -= heroDamage;
                attackHit = true;
            }
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
                Speed = 250;
            }
        }
        public void Eating(Items itemtype)
        {
            Weight -= CollectableItems.Weight[itemtype];
            CurrentHunger = Math.Min(CollectableItems.Food[itemtype],MaxHunger);
            Inventory[itemtype]--;
        }
        public void Drinking(Items itemtype)
        {
            Weight -= CollectableItems.Weight[itemtype];
            CurrentThirst = Math.Min(CollectableItems.Drink[itemtype],MaxThirst);
            Inventory[itemtype]--;
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
        public void Update(TileMap map)
        {
            var keyboardState = Keyboard.GetState();
            bool fDown = keyboardState.IsKeyDown(Keys.F);
            bool fPressed = fDown && !previousState.IsKeyDown(Keys.F);
            Death();
            Attack(keyboardState);

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
            Velocity = Speed * InputManager.Direction;
            Position += Velocity * (float)Globals.Time;

            HitBox = new Rectangle((int)Position.X - Texture.Width / 2,(int)Position.Y - Texture.Height / 2,Texture.Width, Texture.Height);

            if (fPressed)
            {
                Farming(map);
            }

            Debuffs();
            PassiveNeeds();
            previousState = keyboardState;
        }

    }
}

