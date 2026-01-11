using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    public class EvilRabbit : Enemy
    {
        public EvilRabbit(Texture2D texture, Vector2 position) : base(maxHealth : 100, damage : 5,aggroRange : 700f, texture, position)
        { 
        }
        
    }
}
