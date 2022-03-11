using System;

namespace EntityComponentSystem
{
    public interface IComponentManager { }
    public class ComponentManager<T> : IComponentManager where T : IComponent
    {
        public ComponentInstance AddComponent(Entity entity, T component)
        {
            ComponentInstance component_instance;
            component_instance.index = component_data.size;
            component_data.data.Insert(component_instance.index, component);

            entity_map.Add(entity, component_instance);

            component_data.size++;
            return component_instance;
        }

        public IComponent GetComponent(Entity entity)
        {
            ComponentInstance component_instance = entity_map.GetComponent(entity);
            return component_data.data[component_instance.index];
        }

        public void RemoveComponent(Entity entity)
        {
            ComponentInstance component_instance = entity_map.GetComponent(entity);
            ComponentInstance last_component;
            last_component.index = component_data.size - 1;

            component_data.data[component_instance.index] = component_data.data[last_component.index];
            Entity last_entity = entity_map.GetEntity(last_component);

            entity_map.Remove(entity);
            entity_map.Update(last_entity, last_component);
            component_data.size--;
        }

        ComponentData component_data = new ComponentData();
        EntityMap entity_map = new EntityMap();
    }
}
