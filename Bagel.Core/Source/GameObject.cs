using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bagel {
    public class GameObject {
        public GameObject(string name) {
            this.name = name;
            layer_index = 0;
            m_enabled = true;
            m_components = new ComponentsList(this);
            id = _idGenerator++;
        }

        public void Update(GameTime game_time) { m_components.Update(game_time); }

        public void Render() { m_components.Render(); }

        //add existing component
        public T AddComponent<T>(T component) where T : Component {
            component.parent_game_object = this;
            m_components.Add(component);
            return component;
        }

        //add new component. this ensures components always have a parent game object
        public T AddComponent<T>() where T : Component {
            var component = (T)Activator.CreateInstance(typeof(T), this);
            m_components.Add(component);
            return component;
        }

        //checks if a certain component exists
        public bool HasComponent<T>() where T : Component => m_components.GetComponent<T>() != null;

        //returns true if component is removed else false
        public bool RemoveComponent(Component component) => m_components.Remove(component);

        //checks if a component exists on our game object and removes it if so
        public bool RemoveComponent<T>() where T : Component {
            var component = GetComponent<T>();
            if (component == null) {
                return false;
            }

            return RemoveComponent(component);
        }

        //returns a component if it exists or null
        public T GetComponent<T>() where T : Component => m_components.GetComponent<T>();

        //removes all components
        public void RemoveAllComponents() { m_components.Clear(); }

        //game object and all components are set to value
        public bool enabled {
            get => m_enabled;
            set {
                if (m_enabled == value) {
                    return;
                }

                m_enabled = value;
                m_components.Enabled(value);
            }
        }

        private static uint _idGenerator;
        public int layer_index { get; set; }
        public uint id { get; }
        public string name { get; set; }
        private bool m_enabled;
        private ComponentsList m_components;

        //TODO: transform
    }
}