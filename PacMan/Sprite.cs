﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Sprite : GameObject
    {
        public Texture2D Tex { get; private set; }
        public Color Tint { get; private set; }

        public ScreenManager ScreenManager { get; private set; }
        public Sprite(Texture2D tex, Color tint, Vector2 pos, Vector2 size)
            : base(pos, size)
        {
            Tex = tex;
            Tint = tint;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Tex, HitBox, Tint);
        }
    }
}
