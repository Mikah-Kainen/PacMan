using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public abstract class Screen
    {
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public ContentManager ContentManager { get; private set; }
        public ScreenManager ScreenManager { get; private set; }
        public InputManager InputManager { get; private set; }

        public Rectangle Bounds { get; private set; }
        public Screen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            Bounds = bounds;

            GraphicsDeviceManager = graphics;
            ContentManager = content;
            ScreenManager = screenManager;
            InputManager = inputManager;
        }

        public abstract void Load();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
