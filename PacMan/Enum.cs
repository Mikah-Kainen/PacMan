using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public static class Enum
    {
        public enum Directions
        {
            Up,
            Down,
            Left,
            Right,
            None,
        };


        public enum TileType
        {
            Wall,
            Background,
        }
    }
}
