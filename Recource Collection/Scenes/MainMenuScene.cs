using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recource_Collection.Scenes
{
    public class MainMenuScene : Scene
    {
        private SpriteFont _font;
        private Texture2D _pixel;
        private KeyboardState previousState;

        private string[] _buttons =
        {
            "Load Game",
            "Settings",
            "stats",
            "Recipies",
            "Quit"
        };
        private int buttonSelected = 0;

        public MainMenuScene(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Font");
            _pixel = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public override void OnSwitch()
        {
            previousState = Keyboard.GetState();
            buttonSelected = 0;
        }

        public override void Update()
        {
            var keyboardState = Keyboard.GetState();
            bool upPressed = keyboardState.IsKeyDown(Keys.Up) && !previousState.IsKeyDown(Keys.Up);
            bool downPressed = keyboardState.IsKeyDown(Keys.Down) && !previousState.IsKeyDown(Keys.Down);
            bool enterPressed = keyboardState.IsKeyDown(Keys.Enter) && !previousState.IsKeyDown(Keys.Enter);
            

            if (enterPressed)
            {
                SceneManager.SwitchScene(SceneName.Game);
            }
            if (upPressed)
                buttonSelected = Math.Max(0, buttonSelected - 1);

            if (downPressed)
                buttonSelected = Math.Min(_buttons.Length - 1, buttonSelected + 1);

            if (enterPressed)
                ActivateButton();

            previousState = keyboardState;
        }

        private void ActivateButton()
        {
            switch (buttonSelected)
            {
                case 0:
                    SceneManager.SwitchScene(SceneName.Game);
                    break;

                case 1:
                    SceneManager.SwitchScene(SceneName.Settings);
                    Console.WriteLine("Settings selected");
                    break;

                case 2:
                    // SceneManager.SwitchScene(SceneName.Stats);
                    Console.WriteLine("Stats selected");
                    break;
                case 3:
                    SceneManager.SwitchScene(SceneName.Recipes);
                    break;
                case 4:
                    Globals.QuitGame.Invoke();
                    break;
            }
        }

        public override void Draw()
        {
            var _spriteBatch = Globals.SpriteBatch;

            _spriteBatch.Begin();
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.Black * 0.6f);
            _spriteBatch.DrawString(_font, "Main Menu", new Vector2(100, 100), Color.White);

            int Y = 140;

            for (int i = 0; i < _buttons.Length; i++)
            {
                bool selected = i == buttonSelected;


                Color boxColor = selected ? Color.Green : Color.Black;

                Rectangle box = new Rectangle(50, Y + i * 70, 300, 50);

                _spriteBatch.Draw(_pixel, box, boxColor * 0.8f);
                _spriteBatch.DrawString(_font, _buttons[i], new Vector2(box.X + 15, box.Y + 10), Color.White);
            }


            _spriteBatch.End();
        }
    }
}
