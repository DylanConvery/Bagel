using Components;
using DefaultEcs;
using DefaultEcs.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics_World = tainicom.Aether.Physics2D.Dynamics.World;

namespace Systems
{
    public class WorldPhysicsSystem : ISystem<float>
    {
        Physics_World physics_world;

        public WorldPhysicsSystem(World world)
        {
            physics_world = world.Get<Physics_World>();
        }

        public bool IsEnabled { get; set; } = true;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            physics_world.Step(deltaTime);
        }
    }
}
