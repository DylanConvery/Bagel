using Microsoft.Xna.Framework;
using DefaultEcs;
using DefaultEcs.System;

namespace Systems
{
    public class PlayerInputSystem : AEntitySetSystem<float>
    {
        public PlayerInputSystem(World world, GameWindow window) : base(world)
        {

        }
    }
}