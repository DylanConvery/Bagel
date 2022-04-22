﻿using System;
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

            sprite.source = animation.rectangle;
            sprite.origin = new Vector2(animation.rectangle.Width / 2, animation.rectangle.Height / 2);

            if (animation.timer >= animation.frame_speed)
            {
                animation.rectangle.X += animation.offset;
                animation.current_frame++;
                animation.timer = 0;
            }

            if (animation.current_frame >= animation.frame_count)
            {
                animation.rectangle.X = 0;
                animation.current_frame = 0;
            }

            animation.timer += deltaTime;

        }
    }
}
