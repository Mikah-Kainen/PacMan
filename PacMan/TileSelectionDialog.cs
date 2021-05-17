﻿using Microsoft.Xna.Framework;
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
        public TileType SelectedType { get; set; }
        public TileSelectionDialog(Vector2 pos, Vector2 size, Vector2 scale, Vector2 origin, GraphicsDeviceManager graphicsDeviceManager, InputManager input, ContentManager content)
            : base(pos, size, scale, origin)
        {
            var buttonTypes = (TileType[])Enum.GetValues(typeof(TileType));

            Buttons = new List<Button>();
            Texts = new List<Text>();
            Vector2 incriment = new Vector2(0, HitBox.Height / buttonTypes.Length);
            Vector2 currentPos = pos;
            Size = new Vector2(HitBox.Width, HitBox.Height / buttonTypes.Length);

            Texture2D whitePixel = Color.White.CreatePixel(graphicsDeviceManager.GraphicsDevice);
            SpriteFont font = content.Load<SpriteFont>("Font");
            foreach (TileType type in buttonTypes)
            {
                Buttons.Add(new Button(whitePixel, Color.White, currentPos, Size, Vector2.Zero, input)
                {
                    Tag = type
                });
                Texts.Add(new Text(currentPos, Vector2.One, Size, Vector2.Zero, font, type.ToString(), Color.Black));
                currentPos += incriment;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsVisable)
            {
                foreach (Button button in Buttons)
                {
                    if (button.IsClicked())
                    {
                        SelectedType = (TileType)button.Tag;
                    }
                }
            }
            else
            {
                SelectedType = TileType.None;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisable)
            {
                foreach (Button button in Buttons)
                {
                    button.Draw(spriteBatch);
                }
                foreach (Text text in Texts)
                {
                    text.Draw(spriteBatch);
                }
            }
        }

    }
}
