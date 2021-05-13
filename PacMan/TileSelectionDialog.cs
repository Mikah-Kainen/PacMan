using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class TileSelectionDialog
    {
        public List<Button> Buttons { get; set; }
        public Vector2 Size { get; set; }

        public TileSelectionDialog(GraphicsDeviceManager graphicsDeviceManager, InputManager input)
        {
            string[] types = Enum.GetNames(typeof(TileType));
            Buttons = new List<Button>();
            Vector2 incriment = new Vector2(0, 20);
            Vector2 currentPos = new Vector2(100, 100);
            Size = new Vector2(100, 20);

            Texture2D whitePixel = Color.White.CreatePixel(graphicsDeviceManager.GraphicsDevice);
            foreach(string type in types)
            {
                Buttons.Add(new Button(whitePixel, Color.White, currentPos, Size, Vector2.Zero, input));
                currentPos += incriment;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(Button button in Buttons)
            {
                if(button.IsClicked())
                {

                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in Buttons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
