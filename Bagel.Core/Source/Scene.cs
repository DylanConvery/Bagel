using System;
using System.Linq;
using System.Collections.Generic;
using Bagel.Core.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bagel {
    public abstract class Scene {
        public virtual void Start() { }

        public virtual void Update(GameTime game_time) {}

        public void Draw(SpriteBatch sprite_batch) {}

        public void Destroy() { }

        private GameObjectManager game_object_manager;
    }
}