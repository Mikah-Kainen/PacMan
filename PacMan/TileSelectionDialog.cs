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
        //public List<Button> Buttons { get; set; }
        //public List<Text> Texts { get; set; }
        public List<Label> Labels { get; set; }
        public Vector2 Size { get; set; }
        public TileTypes SelectedType { get; set; }
        public TileTypes CurrentPalletType 
        {

            set 
            { 
                foreach(Label label in Labels)
                {
                    if((TileTypes)label.Tag == value)
                    {
                        label.Tint = Color.Yellow;
                    }
                    else
                    {
                        label.Tint = Color.White;
                    }
                }
            } 
        }
        public TileSelectionDialog(Vector2 pos, Vector2 size, Vector2 scale, Vector2 origin, GraphicsDeviceManager graphicsDeviceManager, InputManager input, ContentManager content)
            : base(pos, size, scale, origin)
        {
            var buttonTypes = (TileTypes[])Enum.GetValues(typeof(TileTypes));

            Labels = new List<Label>();
            Vector2 incriment = new Vector2(0, HitBox.Height / buttonTypes.Length);
            Vector2 currentPos = pos;
            Size = new Vector2(HitBox.Width, HitBox.Height / buttonTypes.Length);

            Texture2D whitePixel = Color.White.CreatePixel(graphicsDeviceManager.GraphicsDevice);
            SpriteFont font = content.Load<SpriteFont>("Font");
            foreach (TileTypes type in buttonTypes)
            {
                if (type != TileTypes.None)
                {
                    Labels.Add(new Label(whitePixel, Color.White, currentPos, Size, Vector2.Zero, input, font, type.ToString(), Color.Black, Vector2.One)
                    {
                        Tag = type
                    });
                    //Texts.Add(new Text(currentPos, Size, Vector2.Zero, font, type.ToString(), Color.Black));
                    currentPos += incriment;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsVisable)
            {
                foreach (Label label in Labels)
                {
                    if (label.IsClicked())
                    {
                        SelectedType = (TileTypes)label.Tag;
                    }
                }
            }
            else
            {
                SelectedType = TileTypes.None;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisable)
            {
                foreach (Label label in Labels)
                {
                    label.Draw(spriteBatch);
                }
            }
        }

    }
}
