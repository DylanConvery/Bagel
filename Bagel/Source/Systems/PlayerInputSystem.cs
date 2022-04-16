using Microsoft.Xna.Framework;
using DefaultEcs;
using DefaultEcs.System;
using Components;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Systems
{
    public class PlayerInputSystem : AComponentSystem<float, PlayerInput>
    {
        public PlayerInputSystem(World world) : base(world) { }
        protected override void Update(float deltaTime, ref PlayerInput component)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.W))
            {
                component.vertical = -1;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                component.vertical = 1;
            }
            else if (keyboard.IsKeyDown(Keys.D))
            {
                component.horizontal = 1;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                component.horizontal = -1;
            }
            else
            {
                component.vertical = 0f;
                component.horizontal = 0f;
            }
        }
    }
}
