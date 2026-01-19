using System;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recource_Collection.Scenes
{
    public class StatsScene : Scene
    {
        private readonly ContentManager _content;
        private SpriteFont _font;
        private Texture2D _pixel;

        private KeyboardState previous;

        private StoringData _save;
        private string _statusText = "";

        private const string SaveFilePath = "Saves/save1.json";

        public StatsScene(ContentManager content)
        {
            _content = content;
        }

        public override void OnSwitch()
        {
            _font = _content.Load<SpriteFont>("Font");
            _pixel = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            previous = Keyboard.GetState();
            LoadStats();
        }

        private void LoadStats()
        {
            try
            {
                if (!FileManager.FileExists(SaveFilePath))
                {
                    _save = null;
                    _statusText = "No save found";
                    return;
                }

                string json = FileManager.ReadData(SaveFilePath);
                _save = JsonSerializer.Deserialize<StoringData>(json);

                if (_save == null)
                    _statusText = "Save file exists but can not be read.";
                else
                    _statusText = "";
            }
            catch (Exception ex)
            {
                _save = null;
                _statusText = "Failed to load stats: " + ex.Message;
            }
        }

        public override void Update()
        {
            var keyboardState = Keyboard.GetState();
            if ((keyboardState.IsKeyDown(Keys.Escape) && !previous.IsKeyDown(Keys.Escape)) || (keyboardState.IsKeyDown(Keys.Back) && !previous.IsKeyDown(Keys.Back)))SceneManager.SwitchScene(SceneName.MainMenu);

            if (keyboardState.IsKeyDown(Keys.R) && !previous.IsKeyDown(Keys.R))
                LoadStats();

            previous = keyboardState;
        }

        public override void Draw()
        {
            var spritebatch = Globals.SpriteBatch;

            spritebatch.Begin();

            spritebatch.Draw(_pixel,new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y),Color.Gray);

            spritebatch.DrawString(_font, "Stats", new Vector2(60, 60), Color.White);

            if (!string.IsNullOrEmpty(_statusText))
            {
                spritebatch.DrawString(_font, _statusText, new Vector2(60, 120), Color.Black);
                spritebatch.End();
                return;
            }

            spritebatch.DrawString(_font, "Enemies killed: " + _save.EnemiesKilled, new Vector2(60, 120), Color.White);
            spritebatch.DrawString(_font, "Questions correct: " + _save.QuestionsCorrect, new Vector2(60, 160), Color.White);
            spritebatch.DrawString(_font, "Questions wrong: " + _save.QuestionsIncorrect, new Vector2(60, 200), Color.White);

            spritebatch.DrawString(_font, "World seed: " + _save.WorldSeed, new Vector2(60, 260), Color.Gray);
            spritebatch.DrawString(_font,"Last position: (" + (int)_save.HeroX + ", " + (int)_save.HeroY + ")",new Vector2(60, 300),Color.Gray);

            spritebatch.DrawString(_font, "ESC = Back   R = Refresh", new Vector2(60, Globals.WindowSize.Y - 60), Color.White);

            spritebatch.End();
        }
    }
}
