using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Recource_Collection;
using static AnimationManager;

namespace Recource_Collection
{
    public class Game1 : Game
    {
        private Matrix _translation;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D heroTexture;
        private Hero _hero;
        public Boss _boss;
        Texture2D spriteSheet;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void TranslationCalc()
        {
            _translation = Matrix.CreateTranslation(-_hero.Position.X + Globals.WindowSize.X / 2,- _hero.Position.Y + Globals.WindowSize.Y / 2,0f);

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

            spriteSheet = Content.Load<Texture2D>("treeantSpriteSheet1");
            Globals.SpriteBatch = _spriteBatch;
            heroTexture = Content.Load<Texture2D>("hero");
            _hero = new Hero(100, heroTexture, new Vector2(100, 100));
            _boss = new Boss(spriteSheet, new Vector2(200, 200),300);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Globals.Update(gameTime);
            var keyboardState = Keyboard.GetState();
            TranslationCalc();
            InputManager.Update();
            _hero.Update();
            _boss.Update();

            Debug.WriteLine(_hero.CurrentHealth);
            base.Update(gameTime);


        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(transformMatrix: _translation);

            _hero.Draw();
            _boss.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
