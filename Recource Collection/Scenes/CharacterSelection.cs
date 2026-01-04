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

            foreach (var name in characterTextureNames)
            {
                options.Add(new CharacterOption
                {
                    TextureName = name,
                    Texture = content.Load<Texture2D>(name)
                });
            }
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

            bool leftPressed = keyboardState.IsKeyDown(Keys.Left) && !previousState.IsKeyDown(Keys.Left);
            bool rightPressed = keyboardState.IsKeyDown(Keys.Right) && !previousState.IsKeyDown(Keys.Right);

            bool selectPressed =
                (keyboardState.IsKeyDown(Keys.Enter) || keyboardState.IsKeyDown(Keys.Space)) &&
                !(previousState.IsKeyDown(Keys.Enter) || previousState.IsKeyDown(Keys.Space));

            bool backPressed =
                keyboardState.IsKeyDown(Keys.Back) && !previousState.IsKeyDown(Keys.Back);

            if (leftPressed)
            {
                characterSelected = Math.Max(0, characterSelected - 1);
            }

            if (rightPressed)
            {
                characterSelected = Math.Min(options.Count - 1, characterSelected + 1);
            }

            if (selectPressed)
            {
                Globals.selectedHero = options[characterSelected].TextureName;
                SceneManager.SwitchScene(SceneName.Game);
            }

            if (backPressed)
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

            _spriteBatch.DrawString(_font, "Left/Right to choose, Enter to select,", new Vector2(60, 90), Color.White);

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
