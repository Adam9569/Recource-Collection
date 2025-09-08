using System.Collections.Generic;
using health_management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recource_Collection
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        public QuestionManager _questionsManager;
        public List<Question> _questions;
        public Question _currentQuestion;

        public Texture2D heroTexture;
        private Hero _hero;

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
            heroTexture = Content.Load<Texture2D>("hero");
            _font = Content.Load<SpriteFont>("font");

            _questions = _questionsManager.LoadQuestions("Content/Data/questionsANDanswers.json");
            _currentQuestion = _questions[0];

            _questionsManager = new QuestionManager();
            _hero = new Hero(100, heroTexture, new Vector2(100, 100));
            List<Question> questions = _questionsManager.LoadQuestions("Content/Data/questionsANDanswers.json");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _hero.Update();
            InputManager.Update();


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _hero.Draw();

            if(_currentQuestion != null && _font != null)
            {
                _spriteBatch.DrawString(_font,_currentQuestion.QuestionsTxt,new Vector2(50, 100),Color.Black);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
