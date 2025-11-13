using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private SpriteFont font;
        private WorldItems _worldItems;





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
            Globals.SpriteBatch = _spriteBatch;


            font = Content.Load<SpriteFont>("Font");
            _worldItems = new WorldItems(_spriteBatch);
            _worldItems.LoadContent(Content);

            // TODO: use this.Content to load your game content here

            heroTexture = Content.Load<Texture2D>("hero");
            _hero = new Hero(heroTexture, new Vector2(100, 100));

    }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Globals.Update(gameTime);
            var keyboardState = Keyboard.GetState();
            _hero.Update();
            _worldItems.Update(_hero);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _worldItems.Draw();
            _hero.Draw();
            InputManager.Update();

            int i = 0;
            foreach (var item in _hero.Inventory)
            {
                _spriteBatch.DrawString(font, item.Key + ": " + item.Value, new Vector2(i * 70, 10), Color.Black);
                i++;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
