using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public struct Animation
    {
        public int frame_count;
        public int current_frame;
        public float frame_speed;
        public int offset;
        public Rectangle source_rectangle;
        public float timer;

        public Animation(int frame_count, int current_frame, float frame_speed, int offset, Rectangle source)
        {
            this.frame_count = frame_count;
            this.current_frame = current_frame;
            this.frame_speed = frame_speed;
            this.offset = offset;
            source_rectangle = source;
            timer = 0;
        }
    }
}
