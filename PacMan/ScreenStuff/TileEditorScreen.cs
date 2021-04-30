﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using static PacMan.Enum;

namespace PacMan.ScreenStuff
{
    class TileEditorScreen : Screen
    {
        /// <summary>
        /// ///////////////// SAVEEEEEEE
        /// add button to add pallets (optional)
        /// </summary>
        GraphicsDevice graphicsDevice => GraphicsDeviceManager.GraphicsDevice;
        Point mousePos => InputManager.MouseState.Position;
        Texture2D pixelMap;
        Vector2 tileSize;
        Sprite[,] grid;
        List<Sprite> pallet;
        ColorWheel colorWheel;
        float fraction = 3 / 4f;
        Rectangle gridHitbox;
        Sprite currentPallet;
        //
        public TileEditorScreen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            base.Load(graphics, content, bounds, screenManager, inputManager);
            gridHitbox = new Rectangle(Bounds.Left, Bounds.Top, (int)(Bounds.Width * fraction), (int)(Bounds.Height * fraction));
            currentPallet = null;

            OpenFileDialog dialog = new OpenFileDialog();

            var result = dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                pixelMap = Texture2D.FromStream(graphicsDevice, dialog.OpenFile());
            }

            //define a fraction and multiply bounds by that
            tileSize = new Vector2(bounds.Width * fraction / pixelMap.Width, bounds.Height * fraction / pixelMap.Height);
            Color[] pixels = new Color[pixelMap.Width * pixelMap.Height];
            pixelMap.GetData<Color>(pixels);

            //Now convert this Color[] to a Sprite[,]
            grid = new Sprite[pixelMap.Height, pixelMap.Width];

            for (int y = 0; y < pixelMap.Height; y++)
            {
                for (int x = 0; x < pixelMap.Width; x++)
                {
                    grid[y, x] = pixels[x + y * pixelMap.Width].CreateSprite(GraphicsDeviceManager.GraphicsDevice, tileSize, new Point(x, y));
                }
            }


            Vector2 paintSize = new Vector2(bounds.Width * (1 - fraction) / 5, bounds.Height * (1 - fraction) / 5);
            Vector2 paintOrigin = new Vector2(paintSize.X / 2, paintSize.Y / 2);
            pallet = new List<Sprite>();

            //
            pallet.Add(new Sprite(Color.White.CreatePixel(graphics.GraphicsDevice), Color.White, new Vector2(2 * paintSize.X, 2 * paintSize.Y + bounds.Height * fraction), paintSize, paintOrigin));

            var textureDictionary = ScreenManager.Settings.TextureDictionary;
            int xPos = 1;
            int yPos = 1;
            foreach(var kvp in textureDictionary)
            {
                pallet.Add(new Sprite(Color.White.CreatePixel(graphics.GraphicsDevice), kvp.Key, new Vector2(2 * xPos * paintSize.X, 2 * yPos * paintSize.Y + bounds.Height * fraction), paintSize, paintOrigin));
                xPos++;
                if(xPos > 7)
                {
                    xPos = 1;
                    yPos++;
                }
            }
            for(int i = 0; i < 3; i ++)
            {
                pallet.Add(new Sprite(Color.White.CreatePixel(graphics.GraphicsDevice), Color.White, new Vector2(2 * xPos * paintSize.X, 2 * yPos * paintSize.Y + bounds.Height * fraction), paintSize, paintOrigin));
                xPos++;
                if (xPos > 7)
                {
                    xPos = 1;
                    yPos++;
                }
            }
            colorWheel = new ColorWheel(new Vector2(bounds.Width * fraction * 9/8, bounds.Height * fraction * 9/8), 100, graphics);


            Objects.AddRange(pallet);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (gridHitbox.Contains(mousePos) && currentPallet != null)
                {
                    Sprite spriteToChange = grid[(int)(mousePos.Y / tileSize.Y), (int)(mousePos.X / tileSize.X)];
                    spriteToChange.Tint = currentPallet.Tint;
                }
                else if (colorWheel.GetColor(mousePos).HasValue && currentPallet != null)
                {
                    currentPallet.Tint = colorWheel.GetColor(mousePos).Value;
                }
                else
                {
                    foreach(Sprite sprite in pallet)
                    {
                        if(sprite.HitBox.Contains(mousePos))
                        {
                            currentPallet = sprite;
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach(Sprite sprite in grid)
            {
                sprite?.Draw(spriteBatch);
            }

            colorWheel.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }


        private void ChangeTileColor(Point mousePos, Color newColor)
        {
            Vector2 index = new Vector2(mousePos.X / tileSize.X, mousePos.Y / tileSize.Y);

            if(index.X > pixelMap.Width || index.Y > pixelMap.Height)
            {
                return;
            }
        }
    }
}