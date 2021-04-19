using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        
        /// <summary>
        /// Vector2 pos, Vector2 scale, Point posInGrid
        /// </summary>
        public Dictionary<Color, Func<Vector2, Vector2, Point, Tile>> TextureDictionary { get; private set; }
        public Settings(GraphicsDevice graphicsDevice)
        {
            DirectionDictionary = new Dictionary<Keys, Directions>
            {
                [Keys.Up] = Directions.Up,
                [Keys.Down] = Directions.Down,
                [Keys.Left] = Directions.Left,
                [Keys.Right] = Directions.Right,
                
            };

            Texture2D whitePixel = Color.White.CreatePixel(graphicsDevice);

            //If you want to store this in json
            //Change it to be a Dictionary<Color, (Color, TileType)>
            //Function that takes in a color and tile type and whatever and returns tile

            TextureDictionary = new Dictionary<Color, Func<Vector2, Vector2, Point, Tile>>
            {
                [Color.Black] =              new Func<Vector2, Vector2, Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(whitePixel, Color.Black, scale, TileType.Wall, posInGrid)),
                [new Color(255, 28, 36)] =   new Func<Vector2, Vector2, Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(whitePixel, Color.Red, scale, TileType.Background, posInGrid)),
                [new Color(237, 28, 36)] =   new Func<Vector2, Vector2, Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(whitePixel, Color.Red, scale, TileType.Background, posInGrid)),
                [new Color(34, 177, 76)] =   new Func<Vector2, Vector2, Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(whitePixel, Color.Green, scale, TileType.Background, posInGrid)),
                [new Color(255, 255, 255)] = new Func<Vector2, Vector2, Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(whitePixel, Color.White, scale, TileType.Background, posInGrid)),
            };

        }
    }
}
