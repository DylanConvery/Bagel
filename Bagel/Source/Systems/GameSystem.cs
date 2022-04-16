using Components;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace Systems
{
    public class GameSystem : ISystem<float>
    {
        private DefaultEcs.World world;
        private float spawn_timer;
        private ContentManager content;

        public GameSystem(DefaultEcs.World world, ContentManager content)
        {
            this.world = world;
            this.content = content;
        }

        public bool IsEnabled { get; set; } = true;

        public void Dispose()
        {
        }

        public void Update(float deltaTime)
        {
            if ((spawn_timer -= deltaTime) < 0f)
            {
                spawn_timer = 3f;
                 
                Entity topping = world.CreateEntity();
                topping.Set<topping>();
                topping.Set(new Sprite(content.Load<Texture2D>("box"), 0));
                topping.Set<Physics>();

                ref var topping_sprite = ref topping.Get<Sprite>();
                ref var topping_body = ref topping.Get<Physics>().body;
                topping_body = world.Get<tainicom.Aether.Physics2D.Dynamics.World>().CreateRectangle(
                    32f * topping_sprite.scale.X,
                    38f * topping_sprite.scale.Y,
                    0.1f,
                    new Vector2(100, 200),
                    0f,
                    BodyType.Dynamic
                );

                topping_body.Tag = topping;
            }

            //Debug.WriteLine(world.GetEntities().With<topping>().AsSet().Count);
        }
    }
}
