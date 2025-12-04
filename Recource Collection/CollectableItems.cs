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
        twigs,
        apple,
        waterBottle
    }


    public static class CollectableItems
    {
        public static Dictionary<Items, int> Weight = new Dictionary<Items, int>()
        {
            {Items.banana,1 },
            {Items.sword , 10 },
            {Items.healthPotion ,2},
            {Items.coin , 0 },
            {Items.twigs, 1 },
            {Items.apple, 1},
            {Items.waterBottle ,2 }

        };
        public static Dictionary<Items, int> Food = new Dictionary<Items, int>()
        {
            {Items.banana , 4 },
            {Items.apple , 3}
        };
        public static Dictionary<Items, int> Drink = new Dictionary<Items, int>()
        {
            {Items.waterBottle , 25}
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
