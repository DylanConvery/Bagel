using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace EntityComponentSystem
{
    public abstract class System
    {
        public virtual void Initialize() { }
        public virtual void Update(GameTime delta_time) { }
        public virtual void Draw() { }
        public void RegisterScene(Scene scene)
        {
            this.scene = scene;
        }
        public void registerEntity(Entity entity)
        {
            entities.Add(entity);
        }
        public void unregisterEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        protected BitArray systems = new BitArray(32);
        protected List<Entity> entities = new List<Entity>();
        protected Scene scene;
    }
}
