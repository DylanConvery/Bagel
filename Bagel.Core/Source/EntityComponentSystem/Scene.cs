using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentSystem
{
    public class Scene
    {
        public Scene(EntityManager entity_manager)
        {
            this.entity_manager = entity_manager;
        }

        public void Update(GameTime game_time)
        {
            foreach (System system in systems)
            {
                system.Update(game_time);
            }
        }

        public void Draw()
        {
            foreach (System system in systems)
            {
                system.Draw();
            }
        }

        public EntityHandle CreateEntity()
        {
            Entity entity = entity_manager.CreateEntity();
            EntityHandle entity_handle = new EntityHandle();
            entity_handle.scene = this;
            entity_handle.entity = entity;
            return entity_handle;
        }

        public void RemoveEntity(Entity entity)
        {
            foreach (System system in systems)
            {
                system.unregisterEntity(entity);
            }
            entity_manager.RemoveEntity(entity);
        }

        public void AddSystem(System system)
        {
            system.RegisterScene(this);
            systems.Add(system);
        }

        public void AddComponent<T>(Entity entity, T component) where T : IComponent
        {
            ComponentManager<T> component_manager = GetComponentManager<T>();
            component_manager.AddComponent(entity, component);
        }

        public void RemoveComponent<T>(Entity entity) where T : IComponent
        {
            ComponentManager<T> component_manager = GetComponentManager<T>();
            component_manager.RemoveComponent(entity);
        }

        private ComponentManager<T> GetComponentManager<T>() where T : IComponent
        {
            ComponentManager<T> component_manager = new ComponentManager<T>();
            component_managers.Add(component_manager);
            return component_manager;
        }

        private EntityManager entity_manager;
        private List<System> systems = new List<System>();
        private List<IComponentManager> component_managers = new List<IComponentManager>();
    }
}
