using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class TileSelectionDialog
    {
        public List<Button> Buttons { get; set; }
        public List<Text> Texts { get; set; }
        public Vector2 Size { get; set; }
        string[] buttonNames;

        public TileSelectionDialog(GraphicsDeviceManager graphicsDeviceManager, InputManager input, ContentManager content)
        {
            buttonNames = Enum.GetNames(typeof(TileType));
            Buttons = new List<Button>();
            Texts = new List<Text>();
            Vector2 incriment = new Vector2(0, 20);
            Vector2 currentPos = new Vector2(100, 100);
            Size = new Vector2(100, 20);

            Texture2D whitePixel = Color.White.CreatePixel(graphicsDeviceManager.GraphicsDevice);
            SpriteFont font = content.Load<SpriteFont>("Font");
            foreach (string type in buttonNames)
            {
                Buttons.Add(new Button(whitePixel, Color.White, currentPos, Size, Vector2.Zero, input));
                Texts.Add(new Text(currentPos, Vector2.One, Size, Vector2.Zero, font, type, Color.Black));
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
            foreach(Text text in Texts)
            {
                text.Draw(spriteBatch);
            }
        }
    }
}
