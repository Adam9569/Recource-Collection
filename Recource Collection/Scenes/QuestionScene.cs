using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recource_Collection
{
    public class QuestionScene : Scene
    {
        private readonly ContentManager _content;

        private SpriteFont _font;
        private Texture2D _pixel;
        private Texture2D _cursorTex;
        private Rectangle _textBoxRect;
        private Vector2 _cursorPosition;

        private StringBuilder _inputBuilder = new StringBuilder();
        private int _offset = 10;
        private List<Question> _questions = new List<Question>();
        private Question _currentQuestion;
        private readonly Random _rnd = new Random();

        private KeyboardState _previousKeyboard;
        private bool _registered = false;

        private bool ShowPreviousAns = false;
        private string previousAns = "";

        public QuestionScene(ContentManager content)
        {
            _content = content;

            _font = content.Load<SpriteFont>("Font");

            _pixel = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            _cursorTex = _pixel;

            _textBoxRect = new Rectangle(50, 300, 700, 60);
            _cursorPosition = new Vector2(0, _textBoxRect.Y + 15);
        }

        public override void OnSwitch()
        {
            _previousKeyboard = Keyboard.GetState();

            _questions = QuestionManager.LoadQuestions("Content/Data/questions.json");
            RndQuestion();

            _inputBuilder.Clear();
            ShowPreviousAns = false;

            RegisterTextInput();
        }

        private void RndQuestion()
        {
            if (_questions == null || _questions.Count == 0)
            {
                _currentQuestion = null;
                return;
            }

            _currentQuestion = _questions[_rnd.Next(0, _questions.Count)];
        }

        public override void Update()
        {
            var keyboard = Keyboard.GetState();
            _previousKeyboard = keyboard;
        }

        public override void Draw()
        {
            var _spritebatch = Globals.SpriteBatch;
            _spritebatch.Begin();

            _spritebatch.Draw(_pixel, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.CornflowerBlue);

            if (_currentQuestion == null)
            {
                _spritebatch.End();
                return;
            }

            _spritebatch.DrawString(_font, _currentQuestion.QuestionsTxt, new Vector2(50, 140), Color.White);

            _spritebatch.Draw(_pixel, _textBoxRect, Color.Gray);
            _spritebatch.Draw(_pixel, new Rectangle(_textBoxRect.X, _textBoxRect.Y, _textBoxRect.Width, 2), Color.White);
            _spritebatch.Draw(_pixel, new Rectangle(_textBoxRect.X, _textBoxRect.Bottom , _textBoxRect.Width, 2), Color.White);

            int cursorX = (int)_font.MeasureString(_inputBuilder.ToString()).X;
            _spritebatch.Draw(_cursorTex,new Rectangle(_textBoxRect.X + _offset + cursorX, (int)_cursorPosition.Y, 5, 30),Color.Black);


            _spritebatch.DrawString(_font, _inputBuilder.ToString(), new Vector2(_textBoxRect.X + 10, _textBoxRect.Y + 15), Color.White);

            if (ShowPreviousAns)
            {
                _spritebatch.DrawString(_font, "Answer to the previous question was : " + previousAns, new Vector2(50, 340), Color.Black);
            }

            _spritebatch.End();
        }

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            if (_currentQuestion == null) return;

            char c = e.Character;

            switch (c)
            {
                case '\b':
                    if (_inputBuilder.Length > 0)
                        _inputBuilder.Remove(_inputBuilder.Length - 1, 1);
                    break;

                case '\r':
                case '\n':
                    string answer = _inputBuilder.ToString().Trim();
                    bool correct = string.Equals(answer,_currentQuestion.AnswerTxt.Trim(),StringComparison.OrdinalIgnoreCase);

                    if (correct)
                    {
                        UnregisterTextInput();
                        SceneManager.SwitchScene(SceneName.Game);
                        return;
                    }
                    else
                    {
                        previousAns = _currentQuestion.AnswerTxt;
                        ShowPreviousAns = true;
                        _inputBuilder.Clear();

                        RndQuestion();
                    }
                    break;

                default:
                    if (!char.IsControl(c))
                    {
                        _inputBuilder.Append(c);
                    }
                        
                    break;
            }

            if (_inputBuilder.Length > 40)
            {
                _inputBuilder.Length = 40;
            }
                
        }

        private void RegisterTextInput()
        {
            if (_registered) return;
            Globals.Window.TextInput += OnTextInput;
            _registered = true;
        }

        private void UnregisterTextInput()
        {
            if (!_registered) return;
            Globals.Window.TextInput -= OnTextInput;
            _registered = false;
        }
    }
}
