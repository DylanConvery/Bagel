using Components;
using DefaultEcs;
using DefaultEcs.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{
    public class WorldPhysicsSystem : AComponentSystem<float, tainicom.Aether.Physics2D.Dynamics.World>
    {
        public WorldPhysicsSystem(World world) : base(world) {}

        protected override void Update(float deltaTime, ref tainicom.Aether.Physics2D.Dynamics.World world)
        {
            world.Step(deltaTime);
        }
    }
}
