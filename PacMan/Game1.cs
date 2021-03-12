using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace PacMan
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        Texture2D pixelMap;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixelMap = Content.Load<Texture2D>("pacmanmap");

            MemoryStream stream = new MemoryStream();

            pixelMap.SaveAsPng(stream, pixelMap.Width, pixelMap.Height);

            System.Drawing.Bitmap pixelBitmap = new System.Drawing.Bitmap(stream);

            for(int x = 0; x < pixelBitmap.Width; x++)
            {
                for(int y = 0; y < pixelBitmap.Height; y++)
                {
                    System.Drawing.Color pixelColor = pixelBitmap.GetPixel(x, y);
                    
                    //You now have the pixel color, determine what texture this should map to from your pixel map
                }
            }

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(pixelMap, new Rectangle(10, 10, pixelMap.Width, pixelMap.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
