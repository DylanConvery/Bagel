using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bagel {
    //TODO: allow for child child transforms to be added
    public class Transform : Component {
        public Transform(GameObject game_object) : base(game_object) { SetScale(Vector2.One); }
        public void SetPosition(Vector2 position) { m_position = position; }
        public void SetPosition(float x, float y) { SetPosition(new Vector2(x, y)); }
        public void SetScale(Vector2 scale) { m_scale = scale; }
        public void SetScale(float scale) { SetScale(new Vector2(scale)); }
        public void SetRotation(float rotation) { m_rotation = rotation; }

        //2D X Y position
        public Vector2 Position {
            get => m_position;
            set => SetPosition(value);
        }

        //2D X Y scale
        public Vector2 Scale {
            get => m_scale;
            set => SetScale(value);
        }

        //2D Z rotation
        public float Rotation {
            get => m_rotation;
            set => SetRotation(value);
        }

        private Vector2 m_position;
        private Vector2 m_scale;
        private float m_rotation;
    }
}