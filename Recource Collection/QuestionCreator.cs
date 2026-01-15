using health_management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.IO.Pipes;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Recource_Collection
{
    public class QuestionCreator
    {
        private Texture2D _textBoxTexture;
        private Texture2D _cursorTexture;
        private Vector2 _cursorPostion;
        private SpriteFont _font;
        private Rectangle _textBoxRect;
        private StringBuilder _inputBuilder = new StringBuilder();
        private int offset = 10;
        private string answer;
        private bool drawQuestionBox = false;
        KeyboardState previousKeyBoardState;
        private string previousQ= "";
        private string previousA = "";
        private bool ShowPreviousAns = false;

        public List<Question> _questions;


        public Question _currentQuestion;
        public int CurrentQuestionIndex = 0;
        private Random rnd = new Random();


        private bool _hasFocus = false;
        private static GameWindow _window;
        
        private MouseState _previousMouseState;
        public int HandleCursorPos() => (int)_font.MeasureString(_inputBuilder.ToString()).X;

        public QuestionCreator(GameWindow window, Texture2D textBoxTexture, SpriteFont font, Rectangle textBoxRect, Texture2D cursorTexture,Vector2 cursorPosition)
        {
            _window = window;
            _textBoxTexture = textBoxTexture;
            _font = font;
            _textBoxRect = textBoxRect;
            _cursorPostion = cursorPosition;
            _cursorTexture = cursorTexture;

            _questions = new List<Question>();
        }

        public void LoadContent()
        {
            _questions = new List<Question>();
            _questions = QuestionManager.LoadQuestions("Content/Data/questions.json");

            if (_questions.Count > 0)
            {
                _currentQuestion = _questions[0];
            }
        }
        
        public void NextQuestion()
        {
            if(_questions == null || _questions.Count == 0)
            {
                return;
            }

            CurrentQuestionIndex++;
            if (CurrentQuestionIndex >= _questions.Count)
            {
                CurrentQuestionIndex = 0;
            }
            _currentQuestion = _questions[CurrentQuestionIndex];
        }

        public void CheckAnswer(string answer , string question)
        {
            if (answer == question)
            {
                drawQuestionBox = false;
            }
        }

        private void HandleLength()
        {
            if (_inputBuilder.Length > Globals.MaxChars)
            {
                _inputBuilder.Length = Globals.MaxChars;
            }
        }
        

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            char c = e.Character;

            switch (c)
            {
                case '\b':
                    if (_inputBuilder.Length > 0)
                        _inputBuilder.Remove(_inputBuilder.Length - 1, 1);
                    break;

                case '\r':
                case '\n':
                    answer = _inputBuilder.ToString();
                    _inputBuilder.Length = 0;
                    previousA = _currentQuestion.AnswerTxt;
                    previousQ = _currentQuestion.QuestionsTxt; 

                    if (answer != _currentQuestion.QuestionsTxt)
                    {
                        ShowPreviousAns = true;
                    }
                    NextQuestion();
                    break;
                default:
                    _inputBuilder.Append(c);
                    break;

            }
        }
        private void HandleClick(Point mouseClick)
        {
            if (_textBoxRect.Contains(mouseClick))
            {
                _hasFocus = !_hasFocus;
                if (_hasFocus)
                    RegisterTextInput(OnTextInput);
                else
                    UnRegisterTextInput(OnTextInput);
            }
            else
            {
                if (_hasFocus)
                {
                    _hasFocus = false;
                    UnRegisterTextInput(OnTextInput);
                }
            }
        }


        public void Update()
        {
            var mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            if (_previousMouseState.LeftButton == ButtonState.Released &&
                mouseState.LeftButton == ButtonState.Pressed)
            {
                HandleClick(mouseState.Position);
            }
            if (keyboardState.IsKeyDown(Keys.Tab) && previousKeyBoardState.IsKeyUp(Keys.Tab))
            {
                drawQuestionBox = !drawQuestionBox;
            }

            if (keyboardState.IsKeyDown(Keys.Q) && !previousKeyBoardState.IsKeyDown(Keys.Q))
            {
                if (_questions.Count > 0)
                {
                    int randomIndex = rnd.Next(0, _questions.Count); 
                    _currentQuestion = _questions[randomIndex];          
                }
            }
            

            previousKeyBoardState = keyboardState;
            _previousMouseState = mouseState;
            HandleLength();
        }


        public void Draw(SpriteBatch spriteBatch)
        {

            if (drawQuestionBox)
            {
                if (_currentQuestion != null)
                {
                    spriteBatch.DrawString(_font, _currentQuestion.QuestionsTxt, new Vector2(50, 100), Color.Black);
                }

                spriteBatch.Draw(_textBoxTexture, _textBoxRect, Color.White);
                spriteBatch.DrawString(_font, $"Focus: {_hasFocus}", new Vector2(_textBoxRect.X, _textBoxRect.Y - 30), Color.Yellow);
                spriteBatch.Draw(_cursorTexture, new Rectangle((int)HandleCursorPos() + _textBoxRect.X + offset, (int)_cursorPostion.Y, 4, 30), Color.Black);
                spriteBatch.DrawString(_font, _inputBuilder.ToString(), new Vector2(_textBoxRect.X + 10, _textBoxRect.Y + 10), Color.Red);
                if (ShowPreviousAns)
                {
                    spriteBatch.DrawString(_font,"The answer to the previous question was: " + previousA,new Vector2(50, 160),Color.White);
                }

            }
            
        }

        public string GetInputText() => _inputBuilder.ToString();

        private static void RegisterTextInput(EventHandler<TextInputEventArgs> handler) =>
            _window.TextInput += handler;

        private static void UnRegisterTextInput(EventHandler<TextInputEventArgs> handler) =>
            _window.TextInput -= handler;
    }
}
