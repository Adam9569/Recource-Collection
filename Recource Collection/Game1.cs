using System;
using System.Collections.Generic;
using health_management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Diagnostics;


namespace Recource_Collection
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private Texture2D textBox;
        public Texture2D _cursorTexture;

        public List<Question> _questions;
        public Question _currentQuestion;

        public KeyboardState currentState = Keyboard.GetState();
        public KeyboardState previousState;
        

        public Texture2D heroTexture;
        private Hero _hero;
        public Random rnd = new Random();

        public static GameWindow gw;
        public static MouseState mouseState;
        private QuestionCreator _questionCreator;
        
        private int qOffset = 50;

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
            gw = Window;

           


            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            

            heroTexture = Content.Load<Texture2D>("hero");
            _hero = new Hero(100, heroTexture, new Vector2(Globals.WindowSize.X /2 , Globals.WindowSize.Y /2));


            _font = Content.Load<SpriteFont>("font");
            _cursorTexture = Content.Load<Texture2D>("Cursor");
            textBox = Content.Load<Texture2D>("Textbox");
            _questionCreator = new QuestionCreator(Window,textBox,_font,new Rectangle((int)_hero.Position.X - textBox.Width, (int)_hero.Position.Y + 100, 300, 50),_cursorTexture, new Vector2((int)_hero.Position.X - textBox.Width, (int)_hero.Position.Y + 100));

 

        }
        

        //public int PositiveMod(int num, int mod) => ((num % mod) + mod) % mod;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _hero.Update();
            InputManager.Update();
            _questionCreator.Update();

            
            


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _hero.Draw();
            _questionCreator.Draw(_spriteBatch);

            InputManager.Update();

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
