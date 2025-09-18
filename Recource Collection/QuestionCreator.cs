using health_management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
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

        private bool _hasFocus = false;
        private static GameWindow _window;
        
        private MouseState _previousMouseState;

        public QuestionCreator(GameWindow window, Texture2D textBoxTexture, SpriteFont font, Rectangle textBoxRect, Texture2D cursorTexture,Vector2 cursorPosition)
        {
            _window = window;
            _textBoxTexture = textBoxTexture;
            _font = font;
            _textBoxRect = textBoxRect;
            _cursorPostion = cursorPosition;
            _cursorTexture = cursorTexture;
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

        private void HandleLength()
        {
            if(_inputBuilder.Length > Globals.MaxChars)
            {
                _inputBuilder.Length = Globals.MaxChars;
            }
        }

        public int HandleCursorPos() => (int)_font.MeasureString(_inputBuilder.ToString()).X;

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            char c = e.Character;

            switch (c)
            {
                case '\b':
                    if (_inputBuilder.Length > 0)
                        _inputBuilder.Remove(_inputBuilder.Length - 1, 1);
                    break;

                default:
                    _inputBuilder.Append(c);
                    break;

            }
        }


        public void Update()
        {
            var mouseState = Mouse.GetState();
            if (_previousMouseState.LeftButton == ButtonState.Released &&
                mouseState.LeftButton == ButtonState.Pressed)
            {
                HandleClick(mouseState.Position);
            }

            _previousMouseState = mouseState;
            HandleLength();
            HandleCursorPos();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_textBoxTexture, _textBoxRect, Color.White);
            spriteBatch.DrawString(_font, $"Focus: {_hasFocus}", new Vector2(_textBoxRect.X, _textBoxRect.Y - 30), Color.Yellow);
            spriteBatch.Draw(_cursorTexture, new Rectangle((int)_cursorPostion.X, (int)_cursorPostion.Y, 4, 30), Color.Black);
            spriteBatch.DrawString(_font, _inputBuilder.ToString(), new Vector2(_textBoxRect.X + 10, _textBoxRect.Y + 10), Color.Red);
        }

        public string GetInputText() => _inputBuilder.ToString();

        private static void RegisterTextInput(EventHandler<TextInputEventArgs> handler) =>
            _window.TextInput += handler;

        private static void UnRegisterTextInput(EventHandler<TextInputEventArgs> handler) =>
            _window.TextInput -= handler;
    }
}
