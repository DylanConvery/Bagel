using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentSystem
{
    public class Scene
    {
        public void Initialize()
        {
            foreach (System system in systems)
            {
                system.Initialize();
            }
        }

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

        void AddComponentManager<T>(ComponentManager<T> component_manager) where T : IComponent
        {

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

        public void AddComponent(Entity entity, IComponent component)
        {
            ComponentManager<IComponent> component_manager = GetComponentManager<IComponent>();
            component_manager.AddComponent(entity, component);
        }

        public void RemoveComponent<T>(Entity entity) where T : IComponent
        {
            ComponentManager<T> component_manager = GetComponentManager<T>();
            component_manager.RemoveComponent(entity);
        }

        public void Unpack<T>(Entity entity, ComponentHandle<T> component_handle) where T : IComponent
        {
            var manager = GetComponentManager<T>();
            component_handle = new ComponentHandle<T>(entity, manager);
            //ComponentManager<T> componentManager = component_managers[GetCom]
        }

        private ComponentManager<T> GetComponentManager<T>() where T : IComponent
        {
            //int family = Component<T>.family;
            //if (component_managers.Count > family)
            //{
            //    return (ComponentManager<T>)component_managers[family];
            //    component_managers.
            //}
            //else
            //{
            //    component_managers.Add();
            //}

            //ComponentManager<T> component_manager = new ComponentManager<T>();
            //component_managers.Add(component_manager);
            //return component_manager;
            return null;
        }

        private EntityManager entity_manager;
        private List<System> systems = new List<System>();
        private List<IComponentManager> component_managers = new List<IComponentManager>();
    }
}
