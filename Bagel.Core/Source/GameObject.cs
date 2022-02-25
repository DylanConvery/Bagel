using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagel {
    public class GameObject {
        public GameObject(string name) {
            this.name = name;
            layer_index = 1;
            m_enabled = true;
            id = _idGenerator++;
            m_component_manager = new ComponentManager(this);
            m_transform = new Transform(this);
        }

        public void Update(GameTime game_time) { m_component_manager.Update(game_time); }

        public void Draw(SpriteBatch sprite_batch) { m_component_manager.Draw(sprite_batch); }

        //add existing component
        public T AddComponent<T>(T component) where T : Component {
            component.game_object = this;
            m_component_manager.Add(component);
            return component;
        }

        //add new component. this ensures components always have a parent game object
        public T AddComponent<T>(params dynamic[] args) where T : Component {
            try {
                var component = (T)Activator.CreateInstance(typeof(T), args);
                component.game_object = this;
                m_component_manager.Add(component);
                return component;
            } catch (MissingMethodException e) {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            return null;
        }

        //checks if a certain component exists
        public bool HasComponent<T>() where T : Component => m_component_manager.GetComponent<T>() != null;

        //returns true if component is removed else false
        public bool RemoveComponent(Component component) => m_component_manager.Remove(component);

        //checks if a component exists on our game object and removes it if so
        public bool RemoveComponent<T>() where T : Component {
            var component = GetComponent<T>();
            if (component == null) {
                return false;
            }

            return RemoveComponent(component);
        }

        //returns a component if it exists or null
        public T GetComponent<T>() where T : Component => m_component_manager.GetComponent<T>();

        //removes all components
        public void RemoveAllComponents() { m_component_manager.Clear(); }

        //game object and all components are set to value
        public bool enabled {
            get => m_enabled;
            set {
                if (m_enabled == value) {
                    return;
                }

                m_enabled = value;
                m_component_manager.Enabled(value);
            }
        }

        public Vector2 Position {
            get => m_transform.Position;
            set => m_transform.Position = value;
        }

        public Vector2 Scale {
            get => m_transform.Scale;
            set => m_transform.Scale = value;
        }

        public float Rotation {
            get => m_transform.Rotation;
            set => m_transform.Rotation = value;
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
        private ComponentManager m_component_manager;

        //used for position rotation and scale of all GameObjects
        private Transform m_transform;
    }
}