using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    class Label : Button
    {
        private Text text;

        public Label(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, Vector2 origin, InputManager input, SpriteFont font, string labelText, Color color) : base(tex, tint, pos, scale, origin, input)
        {
            text = new Text(pos, scale, origin, font, labelText, color);
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
