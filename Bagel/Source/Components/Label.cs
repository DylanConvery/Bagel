using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public struct Label
    {
        public SpriteFont font;
        public string text;
        public Vector2 position;
        public Color color;
        public float rotation;
        public Vector2 origin;
        public float scale;
        public float layer;

        public Label(SpriteFont font, string text, Vector2 position)
        {
            this.font = font;
            this.text = text;
            this.position = position;
            color = Color.White;
            rotation = 0;
            origin = Vector2.Zero;
            scale = 1f;
            layer = 0;
        }

        public Label(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, float layer)
        {
            this.font = font;
            this.text = text;
            this.position = position;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.layer = layer;
        }
    }
}
