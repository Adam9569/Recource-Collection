// ShapelessCraftingSystem.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    public static class CraftingRecipiesAndChecker
    {
        public class Recipe
        {
            public string Name;
            public Dictionary<Items, int> Ingredients;
            public Items OutputItem;
            public int OutputAmount;

            public Recipe(string name,Dictionary<Items, int> ingredients,Items outputItem,int outputAmount = 1)
            {
                Name = name;
                Ingredients = ingredients;
                OutputItem = outputItem;
                OutputAmount = outputAmount;
            }
        }
        public static readonly List<Recipe> Recipes = new List<Recipe>
        {
            new Recipe
            (
                name: "Twig Pickaxe",
                ingredients: new Dictionary<Items, int>
                {
                    { Items.twigs, 5 },
                },
                outputItem: Items.TwigPickaxe
            ),

            new Recipe
            (
                name: "Berry bundle",
                ingredients: new Dictionary<Items, int>
                {
                    { Items.berry, 4 }
                },
                outputItem: Items.waterBottle
            ),
            new Recipe
            (
                name: "Rock",
                ingredients: new Dictionary<Items, int>
                {
                    {Items.pebble,4}
                },
                outputItem: Items.Rock
            ),
            new Recipe 
            (
                name: "Rock Pickaxe",
                ingredients: new Dictionary<Items, int>
                {
                    {Items.Rock,3 },
                    {Items.twigs,2 }
                },
                outputItem: Items.RockPickaxe
            )
        };

        private static void UseItems(Items?[] grid, Dictionary<Items, int> ingredients)
        {
            foreach (var requiredItmes in ingredients)
            {
                Items item = requiredItmes.Key;
                int needed = requiredItmes.Value;

                for (int i = 0; i < grid.Length && needed > 0; i++)
                {
                    if (grid[i].HasValue && grid[i].Value == item)
                    {
                        grid[i] = null;
                        needed--;
                    }
                }
            }
        }

        public static bool Craft(Items?[] grid,out Items outputItem,out int outputAmount)
        {
            outputItem = default;
            outputAmount = 0;

            Dictionary<Items, int> gridCounts = new();

            foreach (var slot in grid)
            {
                if (slot == null) continue;

                Items item = slot.Value;
                if (!gridCounts.ContainsKey(item))
                {
                    gridCounts[item] = 0;
                }
                    

                gridCounts[item]++;
            }
            foreach (var recipe in Recipes)
            {
                if (Matches(recipe, gridCounts))
                {
                    UseItems(grid, recipe.Ingredients);

                    outputItem = recipe.OutputItem;
                    outputAmount = recipe.OutputAmount;
                    return true;
                }
            }

            return false;
        }


        private static bool Matches(Recipe recipe,Dictionary<Items, int> gridCounts)
        {
            foreach (var requiredItems in recipe.Ingredients)
            {
                if (!gridCounts.TryGetValue(requiredItems.Key, out int count)) return false;

                if (count < requiredItems.Value)return false;
            }
            return true;
        }

        
    }
}
