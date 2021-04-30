﻿using Microsoft.Xna.Framework;
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
        public Dictionary<Color, TileType> ColorDictionary { get; private set; }

        public string DictionaryPath;
        public Settings(GraphicsDevice graphicsDevice)
        {
            DirectionDictionary = new Dictionary<Keys, Directions>
            {
                [Keys.Up] = Directions.Up,
                [Keys.Down] = Directions.Down,
                [Keys.Left] = Directions.Left,
                [Keys.Right] = Directions.Right,
                
            };



            //Texture2D whitePixel = Color.White.CreatePixel(graphicsDevice);

            //TextureDictionary = new Dictionary<Color, TileType>
            //{
            //    [Color.Black] =              TileType.Wall     ,
            //    [new Color(255, 28, 36)] =   TileType.Background,
            //    [new Color(237, 28, 36)] =   TileType.Background,
            //    [new Color(34, 177, 76)] =   TileType.Background,
            //    [new Color(255, 255, 255)] = TileType.Background,
            //};

            //Read from file

            //bool homeLaptop = false;
            //string filePath = "";
            //if(homeLaptop)
            //{
            //    filePath = @"C:\Users\mikah\source\repos\Mikah-Kainen\PacMan\PacMan";
            //}
            //else
            //{
            //    filePath = @"Z:\PacMan\PacMan";
            //}
            //filePath += @"\Stuffs.txt";

            DictionaryPath = "Stuffs.json";


            var fileToConvetBack = System.IO.File.ReadAllText(DictionaryPath);
            Dictionary<int, TileType> meow = JsonConvert.DeserializeObject<Dictionary<int, TileType>>(fileToConvetBack);

            ColorDictionary = new Dictionary<Color, TileType>();
            foreach (var pair in meow)
            {
                ColorDictionary.Add(pair.Key.FromArgb(), pair.Value);
            }
        }
    }
}
