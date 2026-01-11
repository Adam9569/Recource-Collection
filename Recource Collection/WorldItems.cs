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
        public Texture2D appleTexture;
        public Texture2D waterBottleTexture;
        public Texture2D berryTexture;
        public Texture2D pebbleTexture;
        public Texture2D BerryBundleTexture;
        public Texture2D TwigPickaxeTexture;
        public Texture2D RockPickaxeTexture;
        public Texture2D RockTexture;
        public Texture2D RabbitFlesh;
        public Texture2D CookedRabbitFlesh;
        public Texture2D RabbitHide;


        public Texture2D tree1Tex;
        public Texture2D tree2Tex;
        public Texture2D water1Tex;
        public Texture2D water2Tex;
        public Texture2D rock1Tex;
        public Texture2D rock2Tex;
        public Texture2D grassTex;
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
            appleTexture = content.Load<Texture2D>("apple");
            waterBottleTexture = content.Load<Texture2D>("water");
            berryTexture = content.Load<Texture2D>("berry");
            pebbleTexture = content.Load<Texture2D>("pebble");
            BerryBundleTexture = content.Load<Texture2D>("BerryBundle");
            TwigPickaxeTexture = content.Load<Texture2D>("TwigPickaxe");
            RockPickaxeTexture = content.Load<Texture2D>("RockPickaxe");
            RockTexture = content.Load<Texture2D>("CraftRock");
            RabbitHide = content.Load<Texture2D>("RabbitHide");
            RabbitFlesh = content.Load<Texture2D>("RabbitFlesh");
            CookedRabbitFlesh = content.Load<Texture2D>("CookedRabbitFlesh");


            CollectableItems.inportTextures(twigTexture, Items.twigs);
            CollectableItems.inportTextures(healthPotTexture, Items.healthPotion);
            CollectableItems.inportTextures(swordTexture, Items.sword);
            CollectableItems.inportTextures(coinTexture, Items.coin);
            CollectableItems.inportTextures(appleTexture, Items.apple);
            CollectableItems.inportTextures(waterBottleTexture, Items.waterBottle);
            CollectableItems.inportTextures(berryTexture, Items.berry);
            CollectableItems.inportTextures(pebbleTexture, Items.pebble);
            CollectableItems.inportTextures(RockTexture,Items.Rock);
            CollectableItems.inportTextures(TwigPickaxeTexture, Items.TwigPickaxe);
            CollectableItems.inportTextures(RockPickaxeTexture, Items.RockPickaxe);
            CollectableItems.inportTextures(BerryBundleTexture, Items.BerryBundle);
            CollectableItems.inportTextures(RabbitFlesh, Items.RabbiFlesh);
            CollectableItems.inportTextures(RabbitHide, Items.RabbitHide);
            CollectableItems.inportTextures(CookedRabbitFlesh, Items.CookedRabbitFlesh);

            font = content.Load<SpriteFont>("Font");

            worldItems = new List<Item>
        {
            new Item(Items.twigs, new Vector2(200, 100), new Vector2(48, 48)),
            new Item(Items.sword, new Vector2(400, 200), new Vector2(64, 64)),
            new Item(Items.coin, new Vector2(100, 500), new Vector2(24, 24)),
            new Item(Items.healthPotion, new Vector2(250, 400), new Vector2(48, 48)),
            new Item(Items.apple,new Vector2(350 , 400), new Vector2(48,48)),
            new Item(Items.waterBottle,new Vector2(500,500),new Vector2(64,64))
        };
        }

        public void SpawnItem(Items type, Vector2 position, Vector2 size)
        {
            worldItems.Add(new Item(type, position, size));
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
