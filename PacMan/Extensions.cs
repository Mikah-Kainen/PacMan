using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;


using System.Text;

namespace PacMan
{
    public static class Extensions
    {
        public static Texture2D CreatePixel(this Color color, GraphicsDevice device)
        {
            Texture2D texture = new Texture2D(device, 1, 1);
            texture.SetData(new[] { color });

            return texture;
        }
    }
}
