using Components;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Systems
{
    [With(typeof(TransformComponent))]
    [With(typeof(SpriteComponent))]
    class DrawSystem : AEntitySetSystem<float>
    {
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;

        public DrawSystem(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, World world) : base(world)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
        }

        protected override void PreUpdate(float deltaTime) => spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp);

        protected override void Update(float deltaTime, System.ReadOnlySpan<DefaultEcs.Entity> entities)
        {
            foreach (ref readonly Entity entity in entities)
            {
                ref var sprite_component = ref entity.Get<SpriteComponent>();
                ref var transform = ref entity.Get<TransformComponent>();

                spriteBatch.Draw(
                    sprite_component.texture,
                    ConvertWorldToScreen(transform.body.Position),
                    sprite_component.source,
                    sprite_component.color,
                    transform.body.Rotation,
                    sprite_component.origin,
                    sprite_component.scale,
                    SpriteEffects.None,
                    sprite_component.layerIndex
                );

            }
        }

        public Vector2 ConvertWorldToScreen(Vector2 position)
        {
            Matrix i = Matrix.CreateOrthographicOffCenter(0, 800, 600, 0, 0, 0);  //what I use on a 1920x1080 screen
            Matrix view = Matrix.CreateScale(1);
            Vector3 temp = graphicsDevice.Viewport.Project(new Vector3(position, 0), i, view, Matrix.Identity);

            return new Vector2(temp.X, temp.Y);
        }

        public Vector2 ConvertScreenToWorld(int x, int y)
        {
            Matrix i = Matrix.CreateOrthographicOffCenter(0, 960, 540, 0, -100, 10);  //what I use on a 1920x1080 screen
            Matrix view = Matrix.CreateScale(1);
            Vector3 temp = graphicsDevice.Viewport.Unproject(new Vector3(x, y, 0), i, view, Matrix.Identity);

            return new Vector2(temp.X, temp.Y);
        }

        protected override void PostUpdate(float deltaTime) => spriteBatch.End();
    }
}
