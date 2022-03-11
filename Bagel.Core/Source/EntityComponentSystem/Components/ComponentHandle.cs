namespace EntityComponentSystem
{
    public class ComponentHandle<T> where T : IComponent
    {
        public ComponentHandle(Entity entity, ComponentManager<T> component_manager)
        {
            this.entity = entity;
            this.component = component_manager.GetComponent(entity);
            this.component_manager = component_manager;
        }

        public void RemoveComponent()
        {
            component_manager.RemoveComponent(entity);
        }

        public Entity entity;
        public IComponent component;
        public ComponentManager<T> component_manager;
    }

}
