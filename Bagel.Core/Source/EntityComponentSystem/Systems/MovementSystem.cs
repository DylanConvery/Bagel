using Microsoft.Xna.Framework;
using System.Collections;

namespace EntityComponentSystem
{
    public class MovementSystem : System
    {
        public override void Update(GameTime game_time)
        {
            //entites with transform component and input component
            foreach (Entity entity in entities)
            {
                TransformComponent transform = entity.GetComponent<TransformComponent>();
                
                //multiply by some velocity
                transform.position = new Vector2(0.2f * (float)game_time.ElapsedGameTime.TotalMilliseconds, 0);
            }
        }
    }
}
