using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentSystem
{
    public class SpriteComponent : Component<SpriteComponent>
    {
        public Texture2D texture;

        public override string ToString()
        {
            return base.ToString() + texture.Name;
        }
    }
}
