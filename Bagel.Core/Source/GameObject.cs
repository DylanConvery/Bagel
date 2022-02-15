using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bagel {
    public class GameObject {
        public GameObject(){}
        public void Update(){}
        public void Render(){}

        //add component
        //remove component
        //get component
        //remove all components

        private Vector2 m_position;
        private int m_layer_index;
        private string m_name;
        private uint m_id;
        private bool m_enabled;
        private List<Component> m_components;
    }
}