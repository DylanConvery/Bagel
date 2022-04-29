using Microsoft.Xna.Framework;
using DefaultEcs;
using DefaultEcs.System;
using Components;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

namespace Systems
{
    [With(typeof(Physics), typeof(PlayerSpeed), typeof(PlayerDirection), typeof(Animation))]
    public class PlayerMovementSystem : AEntitySetSystem<float>
    {
        Vector2 velocity = Vector2.Zero;

        public PlayerMovementSystem(World world) : base(world) { }
        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var transform = ref entity.Get<Physics>();
            ref var playerSpeed = ref entity.Get<PlayerSpeed>();
            ref var playerInput = ref entity.Get<PlayerDirection>();
            ref var animation = ref entity.Get<Animation>();

            velocity = new Vector2(playerInput.direction.X * playerSpeed.speed, 0);
            transform.body.ApplyLinearImpulse(velocity);

            if (playerInput.direction.X > 0)
            {
                animation.source_rectangle.Y = animation.offset;
            }
            if (playerInput.direction.X < 0)
            {
                animation.source_rectangle.Y = animation.offset * 2;
            }
            if (playerInput.direction.X == 0)
            {
                animation.source_rectangle.Y = 0;
            }
        }
    }
}