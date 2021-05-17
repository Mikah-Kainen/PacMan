using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Pallet : GameObject
    {
        public Sprite PaintContainer { get; set; }
        public TileType TileType { get; set; }
        public Pallet(Sprite sprite, TileType type)
            : base(sprite.Pos, sprite.Size, sprite.Scale, sprite.Origin)
        {
            PaintContainer = sprite;
            TileType = type;
        }

        public override void Update(GameTime gameTime)
        {
            PaintContainer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PaintContainer.Draw(spriteBatch);
        }
    }
}
