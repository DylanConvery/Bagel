using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagel {
    public abstract class Component {
        protected Component(){}
        protected Component(GameObject game_object) => this.game_object = game_object;
        public virtual void Update(GameTime game_time) { }
        public virtual void Draw(SpriteBatch sprite_batch) { }
        //is this component enabled
        public virtual bool enabled { get; set; } = true;
        //reference to parent entity
        public GameObject game_object { get; set; }
    }
}