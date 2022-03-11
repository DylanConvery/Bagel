using System.Collections.Generic;

namespace EntityComponentSystem
{
    public class EntityMap
    {
        public Entity GetEntity(ComponentInstance component_instance)
        {
            return instance_to_entity[component_instance.index];
        }

        public ComponentInstance GetComponent(Entity entity)
        {
            return entity_to_instance_map[entity];
        }

        public void Add(Entity entity, ComponentInstance component_instance)
        {
            entity_to_instance_map.Add(entity, component_instance);
            instance_to_entity.Insert(component_instance.index, entity);
        }

        public void Update(Entity entity, ComponentInstance instance)
        {
            entity_to_instance_map[entity] = instance;
            instance_to_entity[instance.index] = entity;
        }

        public void Remove(Entity entity)
        {
            entity_to_instance_map.Remove(entity);
        }

        private SortedDictionary<Entity, ComponentInstance> entity_to_instance_map = new SortedDictionary<Entity, ComponentInstance>();
        private List<Entity> instance_to_entity = new List<Entity>(1024);
    }
}
