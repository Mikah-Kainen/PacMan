using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using System.Text;

namespace PacMan
{
    public class Pacman : Sprite
    {

        public Pacman(Texture2D tex, Color tint, Vector2 pos, Vector2 size)
            : base(tex, tint, pos, size)
        {

        }


        public override void Update(GameTime gameTime)
        {
            Pos.X++;
            Pos.Y++;
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{

        //}
    }
}
