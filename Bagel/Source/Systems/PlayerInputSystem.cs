using Microsoft.Xna.Framework;
using DefaultEcs;
using DefaultEcs.System;
using Components;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Systems
{
    public class PlayerInputSystem : AComponentSystem<float, PlayerDirection>
    {
        public PlayerInputSystem(World world) : base(world) { }
        protected override void Update(float deltaTime, ref PlayerDirection component)
        {
            KeyboardState keyboard = Keyboard.GetState();
            component.direction = Vector2.Zero;

            if (keyboard.IsKeyDown(Keys.D))
            {
                component.direction.X = 1;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                component.direction.X = -1;
            }
        }
    }
}
