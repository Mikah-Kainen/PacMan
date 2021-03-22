using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Wall : Sprite
    {
        public Wall(Texture2D tex, Color tint, Vector2 pos, Vector2 scale)
            : base(tex, tint, pos, scale)
        {
        }

        public override Sprite Copy(Texture2D tex, Color tint, Vector2 pos, Vector2 scale)
        {
            return new Wall(tex, tint, pos, scale);
        }
    }
}
