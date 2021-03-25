using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

using static PacMan.Enum;

namespace PacMan
{
    public class Tile : Sprite
    {
        public TileType TileType;
        public Tile(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, TileType tileType)
            : base(tex, tint, pos, scale)
        {
            TileType = tileType;
        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
