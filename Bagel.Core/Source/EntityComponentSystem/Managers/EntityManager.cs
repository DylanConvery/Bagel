using System;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentSystem
{
    //change this to use some sort of object pool so we can add and remove entities
    public class EntityManager
    {
        public EntityManager(uint entity_count)
        {
            _entity_count = entity_count;
        }

        public Entity CreateEntity()
        {
            Entity entity;
            entity.id = _entity_count;
            return entity;
        }

        void RemoveEntity(Entity entity)
        {
            
        }

        private uint _entity_count;
    }
}
