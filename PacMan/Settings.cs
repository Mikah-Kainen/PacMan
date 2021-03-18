using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using static PacMan.Enum;

namespace PacMan
{
    public class Settings
    {
        public Dictionary<Keys, Directions> DirectionDictionary { get; private set; }

        public Settings()
        {
            DirectionDictionary = new Dictionary<Keys, Directions>
            {
                [Keys.Up] = Directions.Up,
                [Keys.Down] = Directions.Down,
                [Keys.Left] = Directions.Left,
                [Keys.Right] = Directions.Right,
                
            };

        }
    }
}
