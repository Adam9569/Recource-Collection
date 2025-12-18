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
        private Matrix _translation;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D heroTexture;
        private Hero _hero;
        private SpriteFont font;
        private WorldItems _worldItems;
        private Dictionary<string, Texture2D> Assets;





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
            Assets = new Dictionary<string, Texture2D>();

            Assets.Add("grass", Content.Load<Texture2D>("grass"));
            Assets.Add("rock", Content.Load<Texture2D>("rock"));
            Assets.Add("rock1", Content.Load<Texture2D>("rock1"));
            Assets.Add("rock2", Content.Load<Texture2D>("rock2"));
            Assets.Add("enemy", Content.Load<Texture2D>("enemy"));
            Assets.Add("bush", Content.Load<Texture2D>("bush"));
            Assets.Add("tree1", Content.Load<Texture2D>("tree1"));
            Assets.Add("tree2", Content.Load<Texture2D>("tree2"));
            Assets.Add("twig", Content.Load<Texture2D>("twig"));
            Assets.Add("water1", Content.Load<Texture2D>("water1"));
            Assets.Add("water2", Content.Load<Texture2D>("water2"));
            Assets.Add("water", Content.Load<Texture2D>("water"));

            heroTexture = Content.Load<Texture2D>("hero");
            _hero = new Hero(heroTexture, new Vector2(100, 100));

    }
        private void TranslationCalc()
        {
            _translation = Matrix.CreateTranslation(-_hero.Position.X + Globals.WindowSize.X / 2, -_hero.Position.Y + Globals.WindowSize.Y / 2, 0f);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Globals.Update(gameTime);
            var keyboardState = Keyboard.GetState();
            _hero.Update();
            TranslationCalc();
            _worldItems.Update(_hero);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(transformMatrix: _translation);
            
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
