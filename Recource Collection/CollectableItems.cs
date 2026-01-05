using System;
using System.Collections.Generic;
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
        waterBottle,
        berry,
        pebble,
        Rock,
        RockPickaxe,
        BerryBundle,
        TwigPickaxe,
    }


    public static class CollectableItems
    {
        public static Dictionary<Items, int> Weight = new Dictionary<Items, int>()
        {
            {Items.banana,2 },
            {Items.sword , 10 },
            {Items.healthPotion ,2},
            {Items.coin , 0 },
            {Items.twigs, 1 },
            {Items.apple, 1},
            {Items.waterBottle ,2 },
            {Items.berry,1 },
            {Items.pebble,2 },
            {Items.BerryBundle,5},
            {Items.TwigPickaxe,5 },
            {Items.RockPickaxe,8 }

        };
        public static Dictionary<Items, int> Food = new Dictionary<Items, int>()
        {
            {Items.banana , 4 },
            {Items.apple , 3},
            {Items.berry , 1 },
            {Items.BerryBundle,9 }
        };
        public static Dictionary<Items, int> Drink = new Dictionary<Items, int>()
        {
            {Items.waterBottle , 25},
            {Items.berry , 1 }
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
