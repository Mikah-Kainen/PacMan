using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Food : Sprite
    {


        public Food(Tile foodTile, GraphicsDevice graphicsDevice)
            :base(Color.White.CreatePixel(graphicsDevice), Color.White, new Vector2(foodTile.Pos.X + foodTile.HitBox.Width / 2, foodTile.Pos.Y + foodTile.HitBox.Height / 2), GameScreen.TileSize * 1/6f, new Vector2(.5F, .5F))
        {
            if(foodTile.TileType != TileTypes.Background)
            {
            }
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
