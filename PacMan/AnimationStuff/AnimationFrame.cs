using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public struct AnimationFrame
    {
        public Rectangle HitBox { get; set; }
        public Vector2 Origin { get; set; }

        public AnimationFrame(Rectangle hitbox, Vector2 origin)
        {
            HitBox = hitbox;
            Origin = origin;
        }
    }
}
