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
        public Text(Vector2 pos, Vector2 size, Vector2 scale, Vector2 origin, SpriteFont font, string text, Color color)
            : base(pos, size, scale, origin)
        {
            Font = font;
            Message = text;
            Color = color;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Message, new Vector2(100, 100), Color);
        }
    }
}
