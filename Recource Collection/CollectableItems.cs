using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{

     public enum Items
    {
        banana,
        sword,
        healthPotion,
        coin,
        twigs
    }


    public static class CollectableItems
    {
        public static Dictionary<Items, int> Weight = new Dictionary<Items, int>()
        {
            {Items.banana,1 },
            {Items.sword , 10 },
            {Items.healthPotion ,2},
            {Items.coin , 0 },
            {Items.twigs, 1 }

        };
        public static Dictionary<Items, Texture2D> itemTextures = new Dictionary<Items, Texture2D>();


        public static void inportTextures(Texture2D Texture , Items items)
        {
            if(itemTextures.ContainsKey(items))
            {
                itemTextures[items] = Texture;
            }
            else
            {
                itemTextures.Add(items, Texture);
            }
        }
        
            
        
    }


}
