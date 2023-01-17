using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Text : GameObject
    {
        public SpriteFont Font { get; set; }
        public string Message { get; set; }
        public Color Color { get; set; }
        public Vector2 Size { get; set; }
        public Text(Vector2 pos, Vector2 size, Vector2 origin, SpriteFont font, string text, Color color)
            : base(pos, size, Vector2.One, origin)
        {
            Font = font;
            Message = text;
            Color = color;
            Size = size;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Message, Pos, Color, 0, Vector2.Zero, Size, SpriteEffects.None, 1);
        }
    }
}
