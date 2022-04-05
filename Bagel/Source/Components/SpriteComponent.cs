using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    public struct SpriteComponent
    {
        public Texture2D texture;
        public float layerIndex;
        public Color color;
        public Vector2 origin;
        public Vector2 scale;
    }
}
