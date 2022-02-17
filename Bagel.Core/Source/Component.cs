using Microsoft.Xna.Framework;

namespace Bagel {
    public abstract class Component {
        protected Component(GameObject parent_game_object) {
            m_parent_game_object = parent_game_object;
        }

        public virtual void Update(GameTime game_time) { }
        public virtual void Render() { }

        //is this component enabled
        public virtual bool enabled { get; set; } = true;

        public bool SetParent(GameObject parent_game_object) {
            if (parent_game_object == null) {
                return false;
            }

            parent_game_object = m_parent_game_object;
            return true;
        }

        //reference to parent entity
        private GameObject m_parent_game_object;

        //TODO: transform
    }
}