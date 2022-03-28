using Microsoft.Xna.Framework;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Components;
using Microsoft.Xna.Framework.Input;

namespace Systems
{
    public class PlayerInputSystem : AEntitySetSystem<float>
    {
        public PlayerInputSystem(World world) : base(world.GetEntities().With<PlayerComponent>().AsSet())
        {

        }

        protected override void Update(float delta_time, in Entity entity)
        {
            KeyboardState keyboard = Keyboard.GetState();
            ref var transform = ref entity.Get<TransformComponent>();
            ref var player = ref entity.Get<PlayerComponent>();
            if (keyboard.IsKeyDown(Keys.W)){
                transform.position += new Vector2(0, -player.speed * 50 * delta_time);
            }
            else if (keyboard.IsKeyDown(Keys.S)){
                transform.position += new Vector2(0, player.speed * 50 * delta_time);
            }
            else if (keyboard.IsKeyDown(Keys.D))
            {
                transform.position += new Vector2(player.speed * 50 * delta_time, 0);
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                transform.position += new Vector2(-player.speed * 50 * delta_time, 0);
            }

        }
    }
}