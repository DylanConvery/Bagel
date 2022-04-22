using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    public struct Sprite
    {
        public Texture2D texture;
        public float layerIndex;
        public Color color;
        public Vector2 origin;
        public Vector2 scale;
        public Rectangle source;

        public Sprite(Texture2D texture, float layerIndex, Color color, Vector2 origin, Vector2 scale, Rectangle source)
        {
            this.texture = texture;
            this.layerIndex = layerIndex;
            this.color = color;
            this.origin = origin;
            this.scale = scale;
            this.source = source;
        }

        public Sprite(Texture2D texture, float layerIndex, Color color, Vector2 origin, Vector2 scale)
        {
            this.texture = texture;
            this.layerIndex = layerIndex;
            this.color = color;
            this.origin = origin;
            this.scale = scale;
            this.source = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public Sprite(Texture2D texture, float layerIndex, Vector2 origin, Vector2 scale)
        {
            this.texture = texture;
            this.layerIndex = layerIndex;
            this.color = Color.White;
            this.origin = origin;
            this.scale = scale;
            this.source = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public Sprite(Texture2D texture, float layerIndex, Vector2 origin)
        {
            this.texture = texture;
            this.layerIndex = layerIndex;
            this.color = Color.White;
            this.origin = origin;
            this.scale = Vector2.One;
            this.source = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public Sprite(Texture2D texture, float layerIndex)
        {
            this.texture = texture;
            this.layerIndex = layerIndex;
            this.color = Color.White;
            this.origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            this.scale = Vector2.One;
            this.source = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public Sprite(Texture2D texture, float layerIndex, int scale)
        {
            this.texture = texture;
            this.layerIndex = layerIndex;
            this.color = Color.White;
            this.origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            this.scale = new Vector2(scale);
            this.source = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public Sprite(Texture2D texture, float layerIndex, Color color, int scale)
        {
            this.texture = texture;
            this.layerIndex = layerIndex;
            this.color = color;
            this.origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            this.scale = new Vector2(scale);
            this.source = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public Sprite(Texture2D texture, float layerIndex, Color color, Vector2 origin, Rectangle source)
        {
            this.texture = texture;
            this.layerIndex = layerIndex;
            this.color = color;
            this.origin = origin;
            this.scale = Vector2.One;
            this.source = source;
        }
    }
}
