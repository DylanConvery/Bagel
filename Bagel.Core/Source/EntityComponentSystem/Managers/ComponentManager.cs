using System;
using System.Collections.Generic;

namespace EntityComponentSystem
{
    public class ComponentType
    {

    }

    public class ComponentInstance
    {
        public int index;
    }

    public class ComponentData
    {
        public int size = 1;
        public List<ComponentType> data = new List<ComponentType>(1024);
    }

    public class ComponentManager<ComponentType>
    {
        ComponentInstance AddComponent(Entity entity, ComponentType component)
        {
            ComponentInstance componentInstance = new ComponentInstance();
            componentInstance.index = component_data.size;
            component_data.data.Add(component);
        }

        void RemoveComponent<T>(Entity entity)
        {
            entity_map.Remove(entity);
        }

        ComponentType GetComponent(Entity entity)
        {
            return entity_map[entity];
        }

        ComponentData component_data = new ComponentData();
        protected SortedDictionary<Entity, ComponentInstance> entity_map = new SortedDictionary<Entity, ComponentInstance>();
    }
}
