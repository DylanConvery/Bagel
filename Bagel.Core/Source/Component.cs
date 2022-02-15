namespace Bagel {
    public abstract class Component {
        protected Component(GameObject parent) {
            m_parent_game_object = parent;
        }
        public abstract void Update();
        public abstract void Render();

        private GameObject m_parent_game_object;
    }
}