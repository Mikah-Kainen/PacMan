
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace PacMan
{
    public class GameScreen : Screen
    {
        private Texture2D pixelMap;
        private Dictionary<System.Drawing.Color, Texture2D> textureDictionary;
        private List<Sprite> sprites;

        private Rectangle screen => GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds;

        public GameScreen(GraphicsDeviceManager graphics, ContentManager content, int xBound, int yBound)
            :base(graphics, content, xBound, yBound)
        {
            textureDictionary = new Dictionary<System.Drawing.Color, Texture2D>
            {
                [System.Drawing.Color.Black] = CreatePixel(Color.Black),
                [System.Drawing.Color.Red] = CreatePixel(Color.Red),
                [System.Drawing.Color.Green] = CreatePixel(Color.Green),
            };
        }

        public override void Load()
        {
            pixelMap = ContentManager.Load<Texture2D>("pacmanmap");

            MemoryStream stream = new MemoryStream();

            pixelMap.SaveAsPng(stream, pixelMap.Width, pixelMap.Height);

            System.Drawing.Bitmap pixelBitmap = new System.Drawing.Bitmap(stream);
            sprites = new List<Sprite>();

            Vector2 Chunk = new Vector2(10, 10);

            for (int x = 0; x < pixelBitmap.Width; x++)
            {
                for (int y = 0; y < pixelBitmap.Height; y++)
                {
                    System.Drawing.Color pixelColor = pixelBitmap.GetPixel(x, y);
                    sprites.Add(new Sprite(textureDictionary[pixelColor], Color.White, new Vector2(x,y) * Chunk, Chunk));
                    //You now have the pixel color, determine what texture this should map to from your pixel map
                }
            }
            
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixelMap, new Rectangle(10, 10, pixelMap.Width, pixelMap.Height), Color.White);
            foreach(Sprite sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }


        public Texture2D CreatePixel(Color tint)
        {
            Texture2D returnTex = new Texture2D(GraphicsDeviceManager.GraphicsDevice, 1, 1);
            returnTex.SetData(new Color[] {tint});
            return returnTex;
        }
    }
}
