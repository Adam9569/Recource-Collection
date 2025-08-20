using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recource_Collection
{

     public enum Names
    {
        banana,
        sword,
        healthPotion,
        coin,
        twigs
    }
    public static class CollectableItems
    {
        public static Dictionary<Names, int> Weight = new Dictionary<Names, int>()
        {
            {Names.banana,1 },
            {Names.sword , 10 },
            {Names.healthPotion ,2},
            {Names.coin , 0 },
            {Names.twigs, 1 }

        };
    }
}
