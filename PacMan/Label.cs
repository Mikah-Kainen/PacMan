using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Label : Button
    {
        private Text text;

        public Label(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, Vector2 origin, InputManager input, SpriteFont font, string labelText, Color color, Vector2 textScale) : base(tex, tint, pos, scale, origin, input)
        {
            //text = new Text(new Vector2(HitBox.X + HitBox.Width / 2 - labelText.Length * textScale.X, HitBox.Y + HitBox.Height / 2 - textScale.Y), textScale, Vector2.Zero, font, labelText, color);
            text = new Text(pos, textScale, Vector2.Zero, font, labelText, color);
        }

        public override void Update(GameTime gameTime) 
        { 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            text.Draw(spriteBatch);
        }
    }
}
