using Bagel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentSystem
{
    public abstract class System
    {
        public virtual void Update(GameTime delta_time) { }
        public virtual void Draw() { }
        public void RegisterScene(Scene scene) { }
        public void registerEntity(Entity entity) { }
        public void unregisterEntity(Entity entity) { }

        protected List<Entity> _entities = new List<Entity>();
        protected Scene _scene;
    }
}
