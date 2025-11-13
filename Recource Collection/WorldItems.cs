using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    public class WorldItems
    {
        public Texture2D twigTexture;
        public Texture2D swordTexture;
        public Texture2D coinTexture;
        public Texture2D healthPotTexture;
        private List<Item> worldItems;
        private SpriteFont font;
        private SpriteBatch _spriteBatch;


        public WorldItems()
        {

        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
        }

    }
}
