using Microsoft.Xna.Framework;

namespace Bagel {
    public abstract class Component {
        protected Component(GameObject parent_game_object) {
            this.parent_game_object = parent_game_object;
        }

        public virtual void Update(GameTime game_time) { }
        public virtual void Render() { }

        //is this component enabled
        public virtual bool enabled { get; set; } = true;

        //reference to parent entity
        public GameObject parent_game_object { get; set; } = null;

        //TODO: transform
    }
}