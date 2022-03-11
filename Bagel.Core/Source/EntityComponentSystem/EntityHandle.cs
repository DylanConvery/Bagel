namespace EntityComponentSystem
{
    public class EntityHandle
    {
        public void AddComponent<T>(T component) where T : IComponent
        {
            scene.AddComponent(entity, component);
        }

        public void RemoveComponent<T>() where T : IComponent
        {
            scene.RemoveComponent<T>(entity);
        }

        public Scene scene;
        public Entity entity;
    }
}
