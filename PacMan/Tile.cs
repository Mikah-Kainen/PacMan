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

        public List<Tile> Neighbors;

        public Point PosInGrid;

        private Vector2 pos;
        public override ref Vector2 Pos => ref pos;
        public Tile(Texture2D tex, Color tint, Vector2 scale, TileType tileType, Point posInGrid)
            : base(tex, tint, Vector2.Zero, scale, new Vector2(.5f * scale.X * tex.Width, .5f * scale.Y * tex.Height))
        {
            TileType = tileType;
            Neighbors = new List<Tile>();
            this.PosInGrid = posInGrid;

            pos = new Vector2(posInGrid.X * Scale.X + Origin.X, posInGrid.Y * Scale.Y + Origin.Y);
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
