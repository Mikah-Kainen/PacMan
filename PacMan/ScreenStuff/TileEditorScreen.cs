using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace PacMan.ScreenStuff
{
    class TileEditorScreen : Screen
    {
        /// <summary>
        /// Write to new Json file and load from there
        /// add button to add pallets (optional)
        /// </summary>
        
        GraphicsDevice graphicsDevice => GraphicsDeviceManager.GraphicsDevice;
        Point mousePos => InputManager.MouseState.Position;
        Texture2D pixelMap;
        Vector2 tileSize;
        Sprite[,] grid;
        List<Pallet> pallets;
        ColorWheel colorWheel;
        float fraction = 4/5f;
        Rectangle gridHitbox;
        Pallet currentPallet;
        string fileName;
        TileSelectionDialog tileDialog;


        public TileEditorScreen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            base.Load(graphics, content, bounds, screenManager, inputManager);
            gridHitbox = new Rectangle(Bounds.Left, Bounds.Top, (int)(Bounds.Width * fraction), (int)(Bounds.Height * fraction));
            tileDialog = new TileSelectionDialog(new Vector2(gridHitbox.Width + 20, 20), Vector2.One, new Vector2(100, 60), Vector2.Zero, graphics, inputManager, content);
            tileDialog.IsVisable = false;
            Objects.Add(tileDialog);
            currentPallet = null;

            OpenFileDialog dialog = new OpenFileDialog();

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var pacManMapStream = dialog.OpenFile();

                fileName = dialog.FileName;
                pixelMap = Texture2D.FromStream(graphicsDevice, pacManMapStream);

                pacManMapStream.Close();
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
                    Sprite currentSprite = pixels[x + y * pixelMap.Width].CreateSprite(GraphicsDeviceManager.GraphicsDevice, tileSize, new Point(x, y));
                    currentSprite.Tag = ScreenManager.Settings.ColorDictionary[currentSprite.Tint];
                    grid[y, x] = currentSprite;
                }
            }


            Vector2 paintSize = new Vector2(bounds.Width * (1 - fraction) / 5, bounds.Height * (1 - fraction) / 5);
            Vector2 paintOrigin = new Vector2(1 / 2, 1 / 2);
            pallets = new List<Pallet>();

           
            var textureDictionary = ScreenManager.Settings.ColorDictionary;
            int xPos = 1;
            int yPos = 1;
            foreach (var kvp in textureDictionary)
            {
                if (pallets.Count < 14)
                {
                    pallets.Add(new Pallet(new Sprite(Color.White.CreatePixel(graphics.GraphicsDevice), kvp.Key, new Vector2(2 * xPos * paintSize.X, 2 * yPos * paintSize.Y + bounds.Height * fraction), paintSize, paintOrigin), kvp.Value));
                }
                xPos++;
                if (xPos > 7)
                {
                    xPos = 1;
                    yPos++;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                pallets.Add(new Pallet(new Sprite(Color.White.CreatePixel(graphics.GraphicsDevice), Color.White, new Vector2(2 * xPos * paintSize.X, 2 * yPos * paintSize.Y + bounds.Height * fraction), paintSize, paintOrigin), TileTypes.Background));
                xPos++;
                if (xPos > 7)
                {
                    xPos = 1;
                    yPos++;
                }
            }
            colorWheel = new ColorWheel(new Vector2(bounds.Width * fraction * 9 / 8, bounds.Height * fraction * 9 / 8), 100, graphics);

            Objects.AddRange(pallets);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (gridHitbox.Contains(mousePos) && currentPallet.PaintContainer != null)
                {
                    Sprite spriteToChange = grid[(int)(mousePos.Y / tileSize.Y), (int)(mousePos.X / tileSize.X)];
                    spriteToChange.Tint = currentPallet.PaintContainer.Tint;
                    spriteToChange.Tag = currentPallet.TileType;
                }
                else if (colorWheel.GetColor(mousePos).HasValue && currentPallet.PaintContainer != null)
                {
                    currentPallet.PaintContainer.Tint = colorWheel.GetColor(mousePos).Value;
                    currentPallet.TileType = TileTypes.Background;
                }
                else
                {
                    foreach (Pallet pallet in pallets)
                    {
                        if (pallet.PaintContainer.HitBox.Contains(mousePos))
                        {
                            currentPallet = pallet;
                        }
                    }
                }
            }
            if(InputManager.MouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                bool isOnPallet = false;
                foreach(Pallet pallet in pallets)
                {
                    if(pallet.PaintContainer.HitBox.Contains(mousePos))
                    {
                        currentPallet = pallet;
                        isOnPallet = true;
                    }
                }
                if(isOnPallet)
                {
                    tileDialog.IsVisable = true;
                    tileDialog.CurrentPalletType = currentPallet.TileType;
                }
            }
            if (InputManager.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                ScreenManager.Settings.ColorDictionary.Clear();
                ScreenManager.Settings.ColorDictionary.Add(Color.Black, TileTypes.Wall);
                foreach (Sprite pixel in grid)
                {
                    if (!ScreenManager.Settings.ColorDictionary.ContainsKey(pixel.Tint))
                    {
                        ScreenManager.Settings.ColorDictionary.Add(pixel.Tint, (TileTypes)pixel.Tag);
                    }
                }

                Dictionary<int, TileTypes> argbDictionary = new Dictionary<int, TileTypes>();
                foreach (KeyValuePair<Color, TileTypes> kvp in ScreenManager.Settings.ColorDictionary)
                {
                    argbDictionary.Add(kvp.Key.ToArgb(), kvp.Value);
                }
                string serializedDictionary = JsonConvert.SerializeObject(argbDictionary);
                System.IO.File.WriteAllText(ScreenManager.Settings.DictionaryPath, serializedDictionary);


                //1000 / 10
                //1000 / 10
                //generate a 100x100 2d array of colors

                //Create a 2d color array
                //Calculate its size

                int ySize = grid.GetLength(0);
                int xSize = grid.GetLength(1);

                Color[] pixelMap = new Color[grid.Length];

                for (int y = 0; y < ySize; y++)
                {
                    for (int x = 0; x < xSize; x++)
                    {
                        pixelMap[x + y * xSize] = grid[y, x].Tint;
                    }
                }

                Texture2D pixelMapTexture = new Texture2D(graphicsDevice, xSize, ySize);
                pixelMapTexture.SetData(pixelMap);



                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = fileName;
                var readWriteStream = dialog.OpenFile();

                pixelMapTexture.SaveAsPng(readWriteStream, xSize, ySize);
                readWriteStream.Close();

                ScreenManager.LeaveScreen();
                ScreenManager.SetScreen(Screens.Game);
                ScreenManager.CurrentScreen.Init();
            }

            base.Update(gameTime);

            if(tileDialog.SelectedType != TileTypes.None)
            {
                currentPallet.TileType = tileDialog.SelectedType;
                int currentArgb = currentPallet.PaintContainer.Tint.ToArgb();
                foreach(Sprite tile in grid)
                {
                    if(tile.Tint.ToArgb() == currentArgb)
                    {
                        tile.Tag = currentPallet.TileType;
                    }
                }
                if (ScreenManager.Settings.ColorDictionary.ContainsKey(currentPallet.PaintContainer.Tint))
                {
                    ScreenManager.Settings.ColorDictionary[currentPallet.PaintContainer.Tint] = tileDialog.SelectedType;
                }
                tileDialog.IsVisable = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite sprite in grid)
            {
                sprite?.Draw(spriteBatch);
            }

            colorWheel.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }


        private void ChangeTileColor(Point mousePos, Color newColor)
        {
            Vector2 index = new Vector2(mousePos.X / tileSize.X, mousePos.Y / tileSize.Y);

            if (index.X > pixelMap.Width || index.Y > pixelMap.Height)
            {
                return;
            }
        }
    }
}
