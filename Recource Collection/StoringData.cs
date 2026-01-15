using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recource_Collection
{
    public class StoringData
    {
        public int WorldSeed;
        public float HeroX;
        public float HeroY;

        public int EnemiesKilled;

        public int CurrentHealth;
        public int CurrentHunger;
        public int QuestionsCorrect;
        public int QuestionsIncorrect;
        public int CurrentThirst;

        public Dictionary<string, int> Inventory;
    }
}
