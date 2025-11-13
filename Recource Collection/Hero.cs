using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;



namespace Recource_Collection
{
    public class Hero : Sprite
    {
        private const float SPEED = 500;

        public Vector2 Velocity { get; set; }

        public Rectangle HitBox { get; private set; }
        public int Weight { get; set; }
        public int MaxWeight = 100;
        public Dictionary<Items, int> Inventory { get; set; } = new Dictionary<Items, int>();

        public Hero(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Texture = texture;
            Position = position;
            HitBox = new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
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
            var keyboardState = Keyboard.GetState();
            Velocity = SPEED * InputManager.Direction;
            Position += new Vector2(Velocity.X, Velocity.Y) * Globals.Time;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            
        }

    }
}

