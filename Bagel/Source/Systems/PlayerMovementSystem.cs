using Microsoft.Xna.Framework;
using DefaultEcs;
using DefaultEcs.System;
using Components;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

namespace Systems
{
    [With(typeof(Physics))]
    [With(typeof(PlayerMovement))]
    [With(typeof(PlayerInput))]
    [With(typeof(Animation))]
    public class PlayerMovementSystem : AEntitySetSystem<float>
    {
        public PlayerMovementSystem(World world) : base(world) { }
        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var transform = ref entity.Get<Physics>();
            ref var playerMovement = ref entity.Get<PlayerMovement>();
            ref var playerInput = ref entity.Get<PlayerInput>();
            ref var animation = ref entity.Get<Animation>();

            Vector2 velocity = Vector2.Zero;

            velocity.X = Approach(playerInput.horizontal, velocity.X, deltaTime);
            velocity.Y = Approach(playerInput.vertical, velocity.Y, deltaTime);

            transform.body.ApplyLinearImpulse(velocity * playerMovement.speed);

            //TODO: command pattern? right now its slow and inefficient
            if (playerInput.horizontal > 0)
            {
                animation.source_rectangle.Y = animation.offset;
            }
            if (playerInput.horizontal < 0)
            {
                animation.source_rectangle.Y = animation.offset * 2;
            }
            if (playerInput.horizontal == 0)
            {
                animation.source_rectangle.Y = 0;
            }
        }

        float Approach(float flGoal, float flCurrent, float dt)
        {
            float flDifference = flGoal - flCurrent;

            if (flDifference > dt)
                return flCurrent + dt;
            if (flDifference < -dt)
                return flCurrent - dt;

            return flGoal;
        }
    }
}