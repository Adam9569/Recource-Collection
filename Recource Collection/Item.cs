using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    class Item
    {

        Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Items ItemType { get; set; }

        Vector2 Size { get; set; }
        private SpriteBatch _spriteBatch;


        public Item(Items itemType ,Vector2 position, Vector2 size) 
        {
            
            Position = position;
            Size = size;
            ItemType = itemType;
            

            Texture = CollectableItems.itemTextures[ItemType];
        }

        public Rectangle GetHitBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }



        public void Draw(SpriteBatch spriteBatch)
        {
  
            spriteBatch.Draw(Texture,new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),Color.White);
        }

    }
}
