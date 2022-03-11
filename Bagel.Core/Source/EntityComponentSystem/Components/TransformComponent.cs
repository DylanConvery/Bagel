using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentSystem
{
    public class TransformComponent : Component<TransformComponent>
    {
        public Vector2 position;
        public float rotation;
        public float scale;

        public override string ToString()
        {
            return base.ToString() + position.ToString() + ", " + rotation + ", " + scale;
        }
    }
}
