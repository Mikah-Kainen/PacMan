﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Settings
    {
        public const int TileWidth = 60;
        public const int TileHeight = 60;
        public const int TileDialogWidth = 100;
        public const int TileDialogHeight = 120;
        public const bool Editing = true;
        public Dictionary<Keys, Directions> DirectionDictionary { get; private set; }
        /// <summary>
        /// Vector2 pos, Vector2 scale, Point posInGrid
        /// </summary>
        public Dictionary<Color, TileTypes> ColorDictionary { get; private set; }

        public Dictionary<Tile, Tile> TeleportDictionary { get; set; }

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

            TeleportDictionary = new Dictionary<Tile, Tile>();

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

            bool homeLaptop = false;
            string filePath = "";
            if (homeLaptop)
            {
                filePath = @"C:\Users\mikah\source\repos\Mikah-Kainen\PacMan\PacMan\";
            }
            else
            {
                //filePath = @"Z:\Visual Studio 2019\Projects\PacMan\PacMan";
                filePath = @"..\..\..\";
            }

            DictionaryPath = filePath + "Stuffs.json";

            var fileToConvetBack = System.IO.File.ReadAllText(DictionaryPath);
            Dictionary<int, TileTypes> meow = JsonConvert.DeserializeObject<Dictionary<int, TileTypes>>(fileToConvetBack);

            ColorDictionary = new Dictionary<Color, TileTypes>();
            foreach (var pair in meow)
            {
                ColorDictionary.Add(pair.Key.FromArgb(), pair.Value);
            }
        }
    }
}