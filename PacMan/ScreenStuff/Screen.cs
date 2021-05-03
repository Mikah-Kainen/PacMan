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
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public ContentManager ContentManager { get; set; }
        public ScreenManager ScreenManager { get; set; }
        public InputManager InputManager { get; set; }
        public List<GameObject> Objects { get; set; }
        public Rectangle Bounds { get; set; }

        public virtual void Init()
        {

        }

        public void Load(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            Bounds = bounds;

            GraphicsDeviceManager = graphics;
            ContentManager = content;
            ScreenManager = screenManager;
            InputManager = inputManager;

            Objects = new List<GameObject>();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach(var objectOnScreen in Objects)
            {
                objectOnScreen.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var objectOnScreen in Objects)
            {
                objectOnScreen.Draw(spriteBatch);
            }
        }
    }
}
