using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Recource_Collection;

namespace Recource_Collection
{
    public class Game1 : Game
    {
        private Matrix _translation;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D heroTexture;
        private Hero _hero;
        Texture2D spriteSheet;

        int counter;
        int activeFrame;
        int numFrames;


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
            activeFrame = 0;
            numFrames = 2;
            counter = 0;


            spriteSheet = Content.Load<Texture2D>("treeantSpriteSheet1");
            Globals.SpriteBatch = _spriteBatch;
            heroTexture = Content.Load<Texture2D>("hero");
            _hero = new Hero(heroTexture, new Vector2(100, 100));
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
            base.Update(gameTime);

            counter++;
            if(counter > 29)
            {
                counter = 0;
                activeFrame++;

                if(activeFrame == numFrames)
                {
                    activeFrame = 0;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(transformMatrix: _translation);
            _spriteBatch.Draw(spriteSheet, new Rectangle(100,100,200,200),new Rectangle((activeFrame*32),33,32,32),Color.White);
            _hero.Draw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
