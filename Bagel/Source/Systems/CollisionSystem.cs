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
    [With(typeof(Physics))]
    public class CollisionSystem : AEntitySetSystem<float>
    {
        public CollisionSystem(World world) : base(world) { }

        protected override void Update(float deltaTime, System.ReadOnlySpan<DefaultEcs.Entity> entities)
        {

        }
    }
}