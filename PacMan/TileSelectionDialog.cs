using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class TileSelectionDialog : GameObject
    {
        public List<Button> Buttons { get; set; }
        public List<Text> Texts { get; set; }
        public Vector2 Size { get; set; }
        string[] buttonNames;

        public TileSelectionDialog(Vector2 pos, Vector2 size, Vector2 scale, Vector2 origin, GraphicsDeviceManager graphicsDeviceManager, InputManager input, ContentManager content)
            : base(pos, size, scale, origin)
        {
            buttonNames = Enum.GetNames(typeof(TileType));
            Buttons = new List<Button>();
            Texts = new List<Text>();
            Vector2 incriment = new Vector2(0, HitBox.Height / buttonNames.Length);
            Vector2 currentPos = pos;
            Size = new Vector2(HitBox.Width, HitBox.Height / buttonNames.Length);

            Texture2D whitePixel = Color.White.CreatePixel(graphicsDeviceManager.GraphicsDevice);
            SpriteFont font = content.Load<SpriteFont>("Font");
            foreach (string type in buttonNames)
            {
                Buttons.Add(new Button(whitePixel, Color.White, currentPos, Size, Vector2.Zero, input));
                Texts.Add(new Text(currentPos, Vector2.One, Size, Vector2.Zero, font, type, Color.Black));
                currentPos += incriment;
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach(Button button in Buttons)
            {
                if(button.IsClicked())
                {

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
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
