using Components;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Systems
{
    class LabelSystem : AComponentSystem<float, Label>
    {
        private SpriteBatch spriteBatch;
        private World world;

        public LabelSystem(World world, SpriteBatch spriteBatch) : base(world)
        {
            this.spriteBatch = spriteBatch;
            this.world = world;
        }

        protected override void PreUpdate(float deltaTime) => spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        protected override void Update(float deltaTime, ref Label component)
        {
            spriteBatch.DrawString(
                component.font,
                component.text,
                component.position,
                component.color,
                component.rotation,
                component.origin,
                component.scale,
                SpriteEffects.None,
                0);
        }

        protected override void PostUpdate(float deltaTime) => spriteBatch.End();
    }
}
