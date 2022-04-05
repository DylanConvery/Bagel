using Microsoft.Xna.Framework;
using DefaultEcs;
using DefaultEcs.System;
using Components;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

namespace Systems
{
    [With(typeof(TransformComponent))]
    [With(typeof(PlayerMovementComponent))]
    [With(typeof(PlayerInputComponent))]
    public class PlayerMovementSystem : AEntitySetSystem<float>
    {
        public PlayerMovementSystem(World world) : base(world) { }
        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var transform = ref entity.Get<TransformComponent>();
            ref var playerMovement = ref entity.Get<PlayerMovementComponent>();
            ref var playerInput = ref entity.Get<PlayerInputComponent>();

            Vector2 velocity = Vector2.Zero;

            velocity.X = Approach(playerInput.horizontal, velocity.X, deltaTime * playerMovement.speed);
            velocity.Y = Approach(playerInput.vertical, velocity.Y, deltaTime * playerMovement.speed);

            transform.body.ApplyLinearImpulse(velocity);
            Debug.WriteLine(transform.body.Position);
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