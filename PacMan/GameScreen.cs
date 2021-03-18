
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
        private Dictionary<Color, Texture2D> textureDictionary;
        private List<Sprite> sprites;
        private Pacman pacman;
        private Rectangle screen => GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds;

        public GameScreen(GraphicsDeviceManager graphics, ContentManager content, int xBound, int yBound, ScreenManager screenManager, InputManager inputManager)
            :base(graphics, content, xBound, yBound, screenManager, inputManager)
        {
            textureDictionary = new Dictionary<Color, Texture2D>
            {
                [Color.Black] = CreatePixel(Color.Black),
                [new Color(237, 28, 36)] = CreatePixel(Color.Red),
                [new Color(34, 177, 76)] = CreatePixel(Color.Green),
            };
        }

        public override void Load()
        {
            pixelMap = ContentManager.Load<Texture2D>("pacmanmap");

            Color[] pixels = new Color[pixelMap.Width * pixelMap.Height];
            pixelMap.GetData(pixels);

            sprites = new List<Sprite>();

            float xChunk = screen.Width / (float)pixelMap.Width;
            float yChunk = screen.Height / (float)pixelMap.Height;

            Vector2 Chunk = new Vector2(xChunk, yChunk);

            for (int x = 0; x < pixelMap.Width; x++)
            {
                for (int y = 0; y < pixelMap.Height; y++)
                {
                    int index = CalculateIndex(x, y, pixelMap.Width);
                    Color pixelColor = pixels[index];
                    sprites.Add(new Sprite(textureDictionary[pixelColor], Color.White, new Vector2(x,y) * Chunk, Chunk));
                    //You now have the pixel color, determine what texture this should map to from your pixel map
                }
            }

            pacman = new Pacman(CreatePixel(Color.Yellow), Color.White, new Vector2(screen.Width / 2f, screen.Height / 2f), new Vector2(screen.Width / 20f, screen.Height/20f), 500, ScreenManager, InputManager);
            
        }

        public override void Update(GameTime gameTime)
        {
            pacman.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixelMap, new Rectangle(10, 10, pixelMap.Width, pixelMap.Height), Color.White);
            foreach(Sprite sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }
            pacman.Draw(spriteBatch);
        }


        public Texture2D CreatePixel(Color tint)
        {
            Texture2D returnTex = new Texture2D(GraphicsDeviceManager.GraphicsDevice, 1, 1);
            returnTex.SetData(new Color[] {tint});
            return returnTex;
        }

        private int CalculateIndex(int x, int y, int width)
        {
            return width * y + x;
        }
    }
}
