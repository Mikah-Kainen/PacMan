using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public static class Enum
    {
        public enum Directions
        {
            Up = 1,
            Down = -1,
            Left = 2,
            Right = -2,
            None = 0,
        };


        public enum TileType
        {
            Wall,
            Background,
        }

        public enum Screens
        {
            Game,
            Editor,
        }
    }
}
