using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Recource_Collection
{
    public class CharacterSelectionScene : Scene
    {
        private SpriteFont _font;
        private Texture2D _pixel;
        private KeyboardState previousState;


        private class CharacterOption
        {
            public string TextureName;
            public Difficulty Difficulty;
            public Texture2D Texture;
        }

        private List<CharacterOption> options = new List<CharacterOption>();
        private int characterSelected = 0;

        public CharacterSelectionScene(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Font");

            _pixel = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });


            string[] characterTextureNames =
            {
                "hero",
                "Hero2",
                "hero3"
            };

            options.Add(new CharacterOption
            {
                TextureName = "hero",
                Texture = content.Load<Texture2D>("hero"),
                Difficulty = Difficulty.Easy
            });

            options.Add(new CharacterOption
            {
                TextureName = "Hero2",
                Texture = content.Load<Texture2D>("Hero2"),
                Difficulty = Difficulty.Medium
            });

            options.Add(new CharacterOption
            {
                TextureName = "hero3",
                Texture = content.Load<Texture2D>("hero3"),
                Difficulty = Difficulty.Hard
            });
        }

        public override void OnSwitch()
        {
            previousState = Keyboard.GetState();

            int idx = options.FindIndex(o => o.TextureName == Globals.selectedHero);
            characterSelected = (idx >= 0) ? idx : 0;
        }

        public override void Update()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) && !previousState.IsKeyDown(Keys.Left))
            {
                characterSelected = Math.Max(0, characterSelected - 1);
            }

            if (keyboardState.IsKeyDown(Keys.Right) && !previousState.IsKeyDown(Keys.Right))
            {
                characterSelected = Math.Min(options.Count - 1, characterSelected + 1);
            }

            if ((keyboardState.IsKeyDown(Keys.Enter) || keyboardState.IsKeyDown(Keys.Space)) && !(previousState.IsKeyDown(Keys.Enter) || previousState.IsKeyDown(Keys.Space)))
            {
                Globals.selectedHero = options[characterSelected].TextureName;
                Globals.SelectedDifficulty = options[characterSelected].Difficulty;
                SceneManager.SwitchScene(SceneName.Game);
            }

            if (keyboardState.IsKeyDown(Keys.Back) && !previousState.IsKeyDown(Keys.Back))
            {
                SceneManager.BackScene();
            }

            previousState = keyboardState;
        }

        public override void Draw()
        {
            var _spriteBatch = Globals.SpriteBatch;

            _spriteBatch.Begin();

            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.Black * 0.75f);

            _spriteBatch.DrawString(_font, "Left/Right to choose, Enter to select,", new Vector2(50, 80), Color.White);
            _spriteBatch.DrawString(_font, "hero1 = easy , hero2 = medium , hero3 = hard", new Vector2(100, 100), Color.White);

            int startX = 100;
            int y = 200;
            int spacing = 200;

            for (int i = 0; i < options.Count; i++)
            {
                bool selected = (i == characterSelected);

                Rectangle box = new Rectangle(startX + i * spacing, y, 110, 110);
                _spriteBatch.Draw(_pixel, box, selected ? Color.Green : Color.Black * 0.6f);

                Texture2D tex = options[i].Texture;
                Rectangle img = new Rectangle(box.X + 10, box.Y + 10, box.Width - 20, box.Height - 20);
                _spriteBatch.Draw(tex, img, Color.White);

            }

            var selectedTex = options[characterSelected].Texture;
            _spriteBatch.DrawString(_font, "Preview:", new Vector2(60, 340), Color.White);
            _spriteBatch.Draw(selectedTex, new Rectangle(60, 380, 200, 200), Color.White);

            _spriteBatch.End();
        }
    }
}
