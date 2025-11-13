using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recource_Collection
{
    public class WorldItems
    {
        public Texture2D twigTexture;
        public Texture2D swordTexture;
        public Texture2D coinTexture;
        public Texture2D healthPotTexture;
        private List<Item> worldItems;
        private SpriteFont font;
        private SpriteBatch _spriteBatch;

        public WorldItems(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void Update(Hero _hero)
        {
            var keyboardState = Keyboard.GetState();
            if (_hero.Weight >= _hero.MaxWeight)
            {
                return;
            }

            for (int i = worldItems.Count - 1; i >= 0; i--)
            {
                Item item = worldItems[i];

                float distance = Vector2.Distance(_hero.Position, item.Position);

                if (distance < 60 && keyboardState.IsKeyDown(Keys.E))
                {
                    _hero.addToInv(item.ItemType);
                    _hero.Weight += CollectableItems.Weight[item.ItemType];
                    worldItems.RemoveAt(i);
                }
            }
        }
        public void LoadContent(ContentManager content)
        {
            twigTexture = content.Load<Texture2D>("twig");
            healthPotTexture = content.Load<Texture2D>("healthPotion");
            coinTexture = content.Load<Texture2D>("coin");
            swordTexture = content.Load<Texture2D>("sword");
            CollectableItems.inportTextures(twigTexture, Items.twigs);
            CollectableItems.inportTextures(healthPotTexture, Items.healthPotion);
            CollectableItems.inportTextures(swordTexture, Items.sword);
            CollectableItems.inportTextures(coinTexture, Items.coin);

            font = content.Load<SpriteFont>("Font");

            worldItems = new List<Item>
        {
            new Item(Items.twigs, new Vector2(200, 100), new Vector2(48, 48)),
            new Item(Items.sword, new Vector2(400, 200), new Vector2(64, 64)),
            new Item(Items.coin, new Vector2(100, 500), new Vector2(32, 32)),
            new Item(Items.healthPotion, new Vector2(250, 400), new Vector2(32, 32))
        };
        }

        public void Draw()
        {
            foreach (var item in worldItems)
            {
                item.Draw(_spriteBatch);
            }
        }
    }
}
