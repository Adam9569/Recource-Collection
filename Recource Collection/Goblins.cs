using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Recource_Collection
{
    public class Goblins:Enemy
    {

        public Goblins(Texture2D texture, Vector2 position): base(maxHealth: 100, damage: 15, aggroRange: 800f, texture: texture, position: position)
        {
        }

    }
}
