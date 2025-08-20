using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Recource_Collection
{
    public class Sprite
    {
        public Sprite(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
        public static Texture2D Texture { get; set; }
        private static Color Color;
        public Vector2 Position { get; protected set; }
        public Vector2 Origin { get; protected set; }

        
        public void Draw()
        {

            Globals.SpriteBatch.Draw(Texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
