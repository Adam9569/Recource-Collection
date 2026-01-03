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
        public int Weight { get; set; }
        public int CurrentHunger = 50;
        public int CurrentThirst = 50;

        public int MaxWeight = 100;     
        public int MaxHunger = 100;
        public int HungerCounter = 0;
        public int MaxThirst = 100;
        public int ThirstCounter = 0;

        public bool Isfarming { get; set; }
        List<string> FarmingTiles = new List<string>();

        private KeyboardState previousState;
        private string farmTileKey;
        private float farmProgress;
        private float timeNeeded;


        public Dictionary<Items, int> Inventory { get; set; } = new Dictionary<Items, int>();

        public Hero(Texture2D texture, Vector2 position) : base(texture, position)
        {
            HitBox = new Rectangle((int)position.X - Texture.Width /2 , (int)position.Y - Texture.Height /2 , Texture.Width, Texture.Height);
            Speed = 2500;
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
            CurrentHunger += CollectableItems.Food[itemtype];
            Inventory[itemtype]--;
        }
        public void Drinking(Items itemtype)
        {
            Weight -= CollectableItems.Weight[itemtype];
            CurrentThirst += CollectableItems.Drink[itemtype];
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
        private bool TryFarming(TileMap map)
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
                TryFarming(map);
            }
            Debuffs();
            PassiveNeeds();
            previousState = keyboardState;
        }

    }
}

