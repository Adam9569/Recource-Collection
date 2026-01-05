using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recource_Collection
{
    public class RecipeScene : Scene
    {
        private SpriteFont _font;
        private KeyboardState previousState;

        public RecipeScene(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Font");
        }

        public override void OnSwitch() { }

        public override void Update() { }

        public void ReturnToMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
            {
                SceneManager.SwitchScene(SceneName.MainMenu);
            }
        }

        public override void Draw()
        {
            var _spritebatch = Globals.SpriteBatch;
            _spritebatch.Begin();

            int x = 40;
            int y = 40;
            int offset = 30;

            _spritebatch.DrawString(_font, "RECIPES", new Vector2(x, y), Color.White);
            y += offset * 2;

            _spritebatch.DrawString(_font, "Twig Pickaxe:", new Vector2(x, y), Color.White);
            y += offset;
            _spritebatch.DrawString(_font, "- twigs x5 = TwigPickaxe", new Vector2(x, y), Color.White);
            y += offset * 2;

            _spritebatch.DrawString(_font, "Berry Bundle:", new Vector2(x, y), Color.White);
            y += offset;
            _spritebatch.DrawString(_font, "- berry x4 = waterBottle", new Vector2(x, y), Color.White);
            y += offset * 2;

            _spritebatch.DrawString(_font, "Rock:", new Vector2(x, y), Color.White);
            y += offset;
            _spritebatch.DrawString(_font, "- pebble x4 =  Rock", new Vector2(x, y), Color.White);
            y += offset * 2;

            _spritebatch.DrawString(_font, "Rock Pickaxe:", new Vector2(x, y), Color.White);
            y += offset;
            _spritebatch.DrawString(_font, "- Rock x3, twigs x2 = RockPickaxe", new Vector2(x, y), Color.White);

            _spritebatch.End();
        }
    }
}
