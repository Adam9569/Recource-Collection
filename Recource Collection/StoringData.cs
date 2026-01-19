using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recource_Collection
{
    public class StoringData
    {
        public int WorldSeed { get; set; }

        public float HeroX { get; set; }
        public float HeroY { get; set; }

        public int EnemiesKilled { get; set; }

        public int QuestionsCorrect { get; set; }
        public int QuestionsIncorrect { get; set; }

        public int CurrentHealth { get; set; }
        public int CurrentHunger { get; set; }
        public int CurrentThirst { get; set; }

        public Dictionary<string, int> Inventory { get; set; } = new Dictionary<string, int>();
    }
}
