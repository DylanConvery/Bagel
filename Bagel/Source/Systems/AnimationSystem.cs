using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs.System;
using Components;
using DefaultEcs;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Systems
{
    [With(typeof(Animation))]
    [With(typeof(Sprite))]
    public class AnimationSystem : AEntitySetSystem<float>
    {
        public AnimationSystem(World world) : base(world) { }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var animation = ref entity.Get<Animation>();
            ref var sprite = ref entity.Get<Sprite>();


            sprite.source.Offset(32f, 0f);
            animation.current_frame++;

            if(animation.current_frame >= animation.frame_count)
            {
                animation.current_frame = 0;
                sprite.source.Location = new Point(0,0);
            }


            //Debug.WriteLine(sprite.source);

        }
    }
}
