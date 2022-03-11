using Microsoft.Xna.Framework;
using System.Collections;

namespace EntityComponentSystem
{
    public class MovementSystem : System
    {
        MovementSystem()
        {
            systems.Set(1, true);
        }

        public override void Update(GameTime game_time)
        {

            foreach (Entity entity in entities)
            {
                ComponentHandle<TransformComponent> transform = null;
                ComponentHandle<SpriteComponent> sprite = null;
                scene.Unpack<TransformComponent>(entity, transform);
                scene.Unpack<SpriteComponent>(entity, sprite);
                
                //scene.Unpack(entity, transform);
                //transform.position = new Vector2(0.2f * (float)game_time.ElapsedGameTime.TotalMilliseconds, 0);
            }
        }
    }
}
