using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;



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

        public Dictionary<Items, int> Inventory { get; set; } = new Dictionary<Items, int>();

        public Hero(Texture2D texture, Vector2 position) : base(texture, position)
        {
            HitBox = new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
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

        public void Update()
        {
            Velocity = Speed * InputManager.Direction;
            var keyboardState = Keyboard.GetState();
            Position += new Vector2(Velocity.X, Velocity.Y) * Globals.Time;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            Debuffs();
            PassiveNeeds();
        }

    }
}

