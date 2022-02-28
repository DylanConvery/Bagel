using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagel {
    public class Sprite : Component {
        public Sprite(Texture2D texture) { this.m_texture = texture; }
        public override void Update(GameTime game_time) { }

        public override void Draw(SpriteBatch sprite_batch) {
            sprite_batch.Draw(
                m_texture,
                game_object.Position,
                null,
                Color.White,
                game_object.Rotation,
                Vector2.Zero, 
                Vector2.One,
                SpriteEffects.None,
                game_object.layer_index
            );
        }

        //is this component enabled
        public override bool enabled { get; set; } = true;
        private Texture2D m_texture;
    }
}