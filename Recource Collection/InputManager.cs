using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Recource_Collection
{
    public static class InputManager
    {

        public static Vector2 Direction;
        

        public static void Update()
        {
            var keyboardState = Keyboard.GetState();

            Direction = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.W)) Direction.Y--;
            if (keyboardState.IsKeyDown(Keys.S)) Direction.Y++;
            if (keyboardState.IsKeyDown(Keys.A)) Direction.X--;
            if (keyboardState.IsKeyDown(Keys.D)) Direction.X++;
            if (Direction != Vector2.Zero)
            {
                Direction = Vector2.Normalize(Direction);
            }
        }
    }
}
