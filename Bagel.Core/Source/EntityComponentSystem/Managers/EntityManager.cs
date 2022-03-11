using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentSystem
{
    //change this to use some sort of object pool so we can add and remove entities
    public class EntityManager
    {
        public Entity CreateEntity()
        {
            Entity entity = new Entity();
            entity.id = entity_count++;
            entities.Add(entity);
            return entity;
        }

        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
            entity_count--;
        }

        List<Entity> entities = new List<Entity>();
        private uint entity_count;
    }
}
