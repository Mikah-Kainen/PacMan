using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan.ScreenStuff
{
    class StartScreen : Screen
    {
        private Label playButton;
        private Label editMapButton;
        public StartScreen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            base.Load(graphics, content, bounds, screenManager, inputManager);
            playButton = new Label(Color.White.CreatePixel(graphics.GraphicsDevice), Color.Crimson, new Vector2(bounds.Width * .25f, bounds.Width * .25f), new Vector2(bounds.Width * .5f, bounds.Width * .25f), Vector2.Zero, inputManager, content.Load<SpriteFont>("Font"), "Play", Color.Gold, new Vector2(Settings.TileWidth / 10, Settings.TileHeight / 10));
            editMapButton = new Label(Color.White.CreatePixel(graphics.GraphicsDevice), Color.LightBlue, new Vector2(bounds.Width * .25f, bounds.Width * .5f), new Vector2(bounds.Width * .5f, bounds.Width * .25f), Vector2.Zero, inputManager, content.Load<SpriteFont>("Font"), "EditMap", Color.White, new Vector2(Settings.TileWidth / 10, Settings.TileHeight / 10));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            playButton.Draw(spriteBatch);
            editMapButton.Draw(spriteBatch);
        }
    }
}
