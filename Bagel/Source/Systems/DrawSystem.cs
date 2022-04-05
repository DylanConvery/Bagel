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
    [With(typeof(TransformComponent))]
    [With(typeof(SpriteComponent))]
    class DrawSystem : AEntitySetSystem<float>
    {
        private SpriteBatch spriteBatch;

        public DrawSystem(SpriteBatch spriteBatch, World world) : base(world)
        {
            this.spriteBatch = spriteBatch;
        }

        protected override void PreUpdate(float deltaTime) => spriteBatch.Begin(SpriteSortMode.BackToFront);

        protected override void Update(float deltaTime, System.ReadOnlySpan<DefaultEcs.Entity> entities)
        {
            foreach (ref readonly Entity entity in entities)
            {
                ref var sprite_component = ref entity.Get<SpriteComponent>();
                ref var transform = ref entity.Get<TransformComponent>();

                spriteBatch.Draw(
                    sprite_component.texture,
                    transform.body.Position,
                    null,
                    sprite_component.color,
                    transform.body.Rotation,
                    sprite_component.origin,
                    sprite_component.scale,
                    SpriteEffects.None,
                    sprite_component.layerIndex
                );
            }
        }

        protected override void PostUpdate(float deltaTime) => spriteBatch.End();
    }
}
