using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

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
        public Dictionary<Color, (Color color, TileType tileType)> TextureDictionary { get; private set; }
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

            //Func<Vector2, Vector2, Point, Tile>

            TextureDictionary = new Dictionary<Color, (Color color, TileType type)>
            {
                [Color.Black] =              (Color.Black, TileType.Wall     ),
                [new Color(255, 28, 36)] =   (Color.Red,   TileType.Background),
                [new Color(237, 28, 36)] =   (Color.Red,   TileType.Background),
                [new Color(34, 177, 76)] =   (Color.Green, TileType.Background),
                [new Color(255, 255, 255)] = (Color.White, TileType.Background),
            };

            //Read from file

            bool homeLaptop = false;


            string filePath = "";
            if(homeLaptop)
            {
                filePath = @"C:\Users\mikah\source\repos\Mikah-Kainen\PacMan\PacMan";
            }
            else
            {
                filePath = @"Z:\PacMan\PacMan";
            }
            filePath += @"\Stuffs.txt";

            var fileToConvetBack = System.IO.File.ReadAllText(filePath);
            Dictionary<int, (int, TileType)> meow = JsonConvert.DeserializeObject<Dictionary<int, (int, TileType)>>(fileToConvetBack);

            TextureDictionary.Clear();
            foreach (var pair in meow)
            {
                TextureDictionary.Add(pair.Key.FromArgb(), (pair.Value.Item1.FromArgb(), pair.Value.Item2));
            }
        }
    }
}
