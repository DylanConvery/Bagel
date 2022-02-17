using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bagel {
    //TODO: allow for child child transforms to be added
    public class Transform {
        public Transform(GameObject parent_game_object) => m_parent_game_object = parent_game_object;

        //2D X Y position
        private Vector2 m_position;
        //2D X Y scale
        private Vector2 m_scale;
        //2D Z rotation
        private float m_rotation;

        private GameObject m_parent_game_object { get; set; } = null;
    }
}