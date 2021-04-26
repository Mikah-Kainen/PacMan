using Microsoft.Xna.Framework;
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
        GraphicsDevice graphicsDevice => GraphicsDeviceManager.GraphicsDevice;
        Texture2D pixelMap;
        Vector2 tileSize;
        Sprite[,] grid;
        List<Sprite> pallet;
        ColorWheel colorWheel;
        //
        public TileEditorScreen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            base.Load(graphics, content, bounds, screenManager, inputManager);

            OpenFileDialog dialog = new OpenFileDialog();

            var result = dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                pixelMap = Texture2D.FromStream(graphicsDevice, dialog.OpenFile());
            }

            //define a fraction and multiply bounds by that
            float fraction = 3/ 4f;
            tileSize = new Vector2(bounds.Width * fraction / pixelMap.Width, bounds.Height * fraction / pixelMap.Height);
            Color[] pixels = new Color[pixelMap.Width * pixelMap.Height];
            pixelMap.GetData<Color>(pixels);

            //Now convert this Color[] to a Sprite[,]
            grid = new Sprite[pixelMap.Height, pixelMap.Width];

            for (int y = 0; y < pixelMap.Height; y++)
            {
                for (int x = 0; x < pixelMap.Width; x++)
                {
                    grid[y, x] = ScreenManager.Settings.TextureDictionary[pixels[x + y * pixelMap.Width]].CreateTile(GraphicsDeviceManager.GraphicsDevice, tileSize, new Point(x, y));
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
                    xPos = 0;
                    yPos++;
                }
            }
            colorWheel = new ColorWheel(new Vector2(bounds.Width * fraction * 9/8, bounds.Height * fraction * 9/8), 50, graphics);

            Objects.AddRange(pallet);
        }

        public override void Update(GameTime gameTime)
        {

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
