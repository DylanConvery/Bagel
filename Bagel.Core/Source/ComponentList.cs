using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bagel {
    public class ComponentsList {
        public ComponentsList(GameObject parent_game_object) {
            m_parent_game_object = parent_game_object;
            m_components = new List<Component>();
        }

        //adds component to component list
        public void Add(Component component) => m_components.Add(component);

        //tries removes component from component list and returns result
        public bool Remove(Component component) => m_components.Remove(component);

        //removes all comon
        public void Clear() => m_components.Clear();

        public void Update(GameTime game_time) {
            foreach (var component in m_components) {
                if (component.enabled) {
                    component.Update(game_time);
                }
            }
        }

        public void Render() {
            foreach (var component in m_components) {
                if (component.enabled) {
                    component.Render();
                }
            }
        }

        public void Enabled(bool value) {
            foreach (var component in m_components) {
                component.enabled = value;
            }
        }

        //has component
        public T GetComponent<T>() where T : Component {
            foreach (var component in m_components) {
                if (component is T) {
                    return component as T;
                }
            }
            return null;
        }

        public int m_count => m_components.Count;
        private GameObject m_parent_game_object;
        private List<Component> m_components;
    }
}