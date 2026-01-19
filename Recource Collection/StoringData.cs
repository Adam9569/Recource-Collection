using System.Collections.Generic;

namespace Recource_Collection
{
    public class StoringData
    {
        public int WorldSeed { get; set; }
        public int DayCount { get; set; }
        public TimeOfDay TimeOfDay { get; set; }

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
