using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;


using System.Text;

using static PacMan.Enum;

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

        public static Tile CreateTile(this (Color color, TileType tileType) tuple, GraphicsDevice device, Vector2 tileSize, Point posInGrid)
        {
            return new Tile(Color.White.CreatePixel(device), tuple.color, tileSize, tuple.tileType, posInGrid);
        }

        public static int ToArgb(this Color color)
        {
            int iCol = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
            return iCol;
        }

        public static Color FromArgb(this int argb)
        {
            byte alpha = (byte)(argb >> 24);
            byte red = (byte)(argb >> 16);
            byte green = (byte)(argb >> 8);
            byte blue = (byte)(argb);

            return new Color(red, green, blue, alpha);
        }
    }
}
