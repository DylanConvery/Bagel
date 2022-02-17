using System;
using Microsoft.Xna.Framework;

namespace Bagel {
    public class GameObject {
        public GameObject(string name) {
            this.name = name;
            layer_index = 0;
            m_enabled = true;
            id = _idGenerator++;
            m_components = new ComponentList(this);
            m_transform = new Transform(this);
        }

        public void Update(GameTime game_time) { m_components.Update(game_time); }

        public void Render() { m_components.Render(); }

        //add existing component
        public T AddComponent<T>(T component) where T : Component {
            if (component.SetParent(this)) {
                m_components.Add(component);
                return component;
            } else {
                return null;
            }
        }

        //add new component. this ensures components always have a parent game object
        public T AddComponent<T>() where T : Component {
            var component = (T)Activator.CreateInstance(typeof(T), this);
            if (component == null) {
                return null;
            }
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

        //gives each instance of GameObject a unique id
        private static uint _idGenerator;
        //will be used for determining when to draw a GameObject 
        public int layer_index { get; set; }
        //unique id
        public uint id { get; }
        //used as secondary identifier
        public string name { get; set; }
        //determines whether the GameObject is enabled
        private bool m_enabled;
        //list of components
        private ComponentList m_components;
        //used for position rotation and scale of all GameObjects
        private Transform m_transform;
    }
}