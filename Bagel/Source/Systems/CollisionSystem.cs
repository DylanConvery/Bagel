using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using Components;

namespace Bagel.Source.Systems
{
    [With(typeof(TransformComponent))]
    public class CollisionSystem : AEntitySetSystem<float>
    {
        public CollisionSystem(World world) : base(world)
        {

        }
    }
}
