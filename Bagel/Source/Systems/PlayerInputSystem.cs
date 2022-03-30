using Microsoft.Xna.Framework;
using DefaultEcs;
using DefaultEcs.System;
using Components;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Systems
{
    public class PlayerInputSystem : AComponentSystem<float, PlayerInputComponent>
    {
        public PlayerInputSystem(World world) : base(world) { }
        protected override void Update(float deltaTime, ref PlayerInputComponent component)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.W))
            {
                component.vertical = 1005f;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                component.vertical = -1005f;
            }
            else if (keyboard.IsKeyDown(Keys.D))
            {
                component.horizontal = 1005f;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                component.horizontal = -1005f;
            }
            else
            {
                component.vertical = 0f;
                component.horizontal = 0f;

            }


        }
    }
}
