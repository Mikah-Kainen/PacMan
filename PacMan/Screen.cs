﻿using Microsoft.Xna.Framework;
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
        public Screen(GraphicsDeviceManager graphics, ContentManager content, int xBound, int yBound)
        {
            graphics.PreferredBackBufferWidth = xBound;
            graphics.PreferredBackBufferHeight = yBound;
            graphics.ApplyChanges();

            GraphicsDeviceManager = graphics;
            ContentManager = content;
        }

        public abstract void Load();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
