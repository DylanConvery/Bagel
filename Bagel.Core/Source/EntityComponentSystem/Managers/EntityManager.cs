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
            entity.id = _entity_count++;
            entities.Add(entity);
            return entity;
        }

        void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        ArrayList entities = new ArrayList();
        private uint _entity_count;
    }
}
