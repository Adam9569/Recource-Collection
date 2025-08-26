using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recource_Collection
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D heroTexture;
        private Hero _hero;
        public Texture2D twigTexture;
        public Texture2D swordTexture;
        public Texture2D coinTexture;
        public Texture2D healthPotTexture;
        Rectangle twigHitbox;
        private List<Item> worldItems;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Globals.WindowSize = new(1024, 768);
            _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
            _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
            _graphics.ApplyChanges();

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Globals.SpriteBatch = _spriteBatch;
            heroTexture = Content.Load<Texture2D>("hero");
            _hero = new Hero(heroTexture, new Vector2(100, 100));
            twigTexture = Content.Load<Texture2D>("twig");
            healthPotTexture = Content.Load<Texture2D>("healthPotion");
            coinTexture = Content.Load<Texture2D>("coin");
            swordTexture = Content.Load<Texture2D>("sword");
            twigHitbox = new Rectangle(200, 100, twigTexture.Width, twigTexture.Height);
            CollectableItems.inportTextures(twigTexture, Items.twigs);
            CollectableItems.inportTextures(healthPotTexture, Items.healthPotion);
            CollectableItems.inportTextures(swordTexture, Items.sword);
            CollectableItems.inportTextures(coinTexture, Items.coin);


        worldItems = new List<Item>
        {
            new Item(Items.twigs, new Vector2(200, 100), new Vector2(48, 48)),
            new Item(Items.sword, new Vector2(400, 200), new Vector2(64, 64)),
            new Item(Items.coin, new Vector2(100, 500), new Vector2(32, 32)),
            new Item(Items.healthPotion, new Vector2(250, 400), new Vector2(32, 32))
        };
    }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Globals.Update(gameTime);
            var keyboardState = Keyboard.GetState();
            _hero.Update();
            InputManager.Update();

            base.Update(gameTime);


            for (int i = worldItems.Count - 1; i >= 0; i--)
            {
                Item item = worldItems[i];

                float distance = Vector2.Distance(_hero.Position, item.Position);

                if (distance < 60 && keyboardState.IsKeyDown(Keys.E) && _hero.Weight != _hero.MaxWeight)
                {
                    if (_hero.Inventory.ContainsKey(item.ItemType))
                        _hero.Inventory[item.ItemType] += 1;
                    else
                        _hero.Inventory[item.ItemType] = 1;

                    _hero.Weight += CollectableItems.Weight[item.ItemType];
                    worldItems.RemoveAt(i);
                }
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            foreach (var item in worldItems)
            {
                item.Draw(_spriteBatch);
            }
            _hero.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
