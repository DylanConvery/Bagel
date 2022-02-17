using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bagel {
    public class Sprite : Component {
        public Sprite(GameObject parent_game_object) : base(parent_game_object) { }

        public override void Update(GameTime game_time) { }
        public override void Render() { }

        //is this component enabled
        public override bool enabled { get; set; } = true;
    }
}