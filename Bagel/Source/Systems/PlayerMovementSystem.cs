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
    [With(typeof(RigidbodyComponent))]
    public class PlayerMovementSystem : AEntitySetSystem<float>
    {
        public PlayerMovementSystem(World world) : base(world) { }
        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var transform = ref entity.Get<TransformComponent>();
            ref var playerMovement = ref entity.Get<PlayerMovementComponent>();
            ref var rigidbody = ref entity.Get<RigidbodyComponent>();
            ref var playerInput = ref entity.Get<PlayerInputComponent>();

            rigidbody.velocity.X = Approach(playerInput.horizontal, rigidbody.velocity.X, deltaTime * playerMovement.speed);
            rigidbody.velocity.Y = Approach(playerInput.vertical, rigidbody.velocity.Y, deltaTime * playerMovement.speed);

            transform.position += rigidbody.velocity * deltaTime;

            //Debug.WriteLine(rigidbody.velocity);
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