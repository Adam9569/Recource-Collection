using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
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
        public Texture2D swordTexture;
        private Hero _hero;
        public SpriteFont font; 
        public Texture2D pixel;


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

            Globals.SpriteBatch = _spriteBatch;
            heroTexture = Content.Load<Texture2D>("hero");
            swordTexture = Content.Load<Texture2D>("WoodenSword");

            _hero = new Hero(100,100, heroTexture, new Vector2(100, 100));


            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            font = Content.Load<SpriteFont>("Font");
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

            Debug.WriteLine(_hero.CurrentHealth);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(transformMatrix: _translation);
            _spriteBatch.Draw(swordTexture,new Rectangle(100,100,100,100), Color.White);
            _spriteBatch.Draw(swordTexture, new Rectangle(400, 400, 100, 100), Color.White);
            _hero.Draw();

            if (_hero.IsAttacking)
            {
                _spriteBatch.Draw(pixel, _hero.AttackHitbox, Color.Red * 0.5f);
            }

            _spriteBatch.DrawString(font, _hero.CurrentStamina.ToString(), new Vector2(Globals.WindowSize.X - 100, 100), Color.White);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
