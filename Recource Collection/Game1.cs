using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public List<Question> _questions;
        public Question _currentQuestion;

        public KeyboardState currentState = Keyboard.GetState();
        public KeyboardState previousState;
        public int CurrentQuestionIndex = 0;

        public Texture2D heroTexture;
        private Hero _hero;
        public Random rnd = new Random();

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
            heroTexture = Content.Load<Texture2D>("hero");
            _font = Content.Load<SpriteFont>("font");
            _questionsManager = new QuestionManager();


            _questions = new List<Question>();
            _questions = QuestionManager.LoadQuestions("Content/Data/questions.json");

            _hero = new Hero(100, heroTexture, new Vector2(100, 100));

        }
        

        //public int PositiveMod(int num, int mod) => ((num % mod) + mod) % mod;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _hero.Update();
            InputManager.Update();

            if (WasKeyPressed(Keys.Space))
            {

                int _random = rnd.Next(4);


                CurrentQuestionIndex = _random;
            }

            previousState = currentState;
            currentState = Keyboard.GetState();
            
            //if(WasKeyPressed(Keys.A))
            //{
            //    CurrentQuestionIndex = PositiveMod(++CurrentQuestionIndex, _questions.Count);
            //}
            //if (WasKeyPressed(Keys.D))
            //{
            //    CurrentQuestionIndex = PositiveMod(--CurrentQuestionIndex, _questions.Count);
            //}


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public bool WasKeyPressed(Keys key) => !previousState.IsKeyDown(key) && currentState.IsKeyDown(key);
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _hero.Draw();

            _spriteBatch.DrawString(_font, _questions[CurrentQuestionIndex].QuestionsTxt, Vector2.Zero, Color.Red);
            _spriteBatch.DrawString(_font, _questions[CurrentQuestionIndex].AnswerTxt.ToString(), new Vector2(0, _font.MeasureString(_questions[0].QuestionsTxt).Y), Color.Red);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
