using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class GridCell : Sprite
    {

        public GridCell(Texture2D tex, Color tint, Vector2 pos, Vector2 scale)
            : base(tex, tint, pos, scale)
        {

        }
    }
}
