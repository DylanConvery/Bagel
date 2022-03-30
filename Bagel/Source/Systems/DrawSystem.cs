using Components;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Systems
{
    class DrawSystem : AEntitySetSystem<float>
    {
        private SpriteBatch spriteBatch;

        public DrawSystem(SpriteBatch spriteBatch, World world) : base(world.GetEntities().With<TransformComponent>().With<SpriteComponent>().AsSet())
        {
            this.spriteBatch = spriteBatch;
        }

        protected override void PreUpdate(float deltaTime) => spriteBatch.Begin(SpriteSortMode.BackToFront);

        protected override void Update(float deltaTime, System.ReadOnlySpan<DefaultEcs.Entity> entities)
        {
            foreach (ref readonly Entity entity in entities)
            {
                ref var sprite_component = ref entity.Get<SpriteComponent>();
                ref var transform_component = ref entity.Get<TransformComponent>();

                spriteBatch.Draw(
                    sprite_component.texture,
                    transform_component.position,
                    null,
                    sprite_component.color,
                    transform_component.rotation,
                    new Vector2(0, 0),
                    transform_component.scale,
                    SpriteEffects.None,
                    sprite_component.layerIndex
                );
            }
        }

        protected override void PostUpdate(float deltaTime) => spriteBatch.End();
    }
}
