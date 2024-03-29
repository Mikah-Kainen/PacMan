﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public abstract class GameObject
    {
        private Vector2 position;
        public virtual ref Vector2 Pos => ref position;
        public Vector2 Size { get; internal set; }

        private Vector2 origin;

        public Vector2 Origin { get; set; }

        public bool IsVisable { get; set; }
        public virtual Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)Math.Round((Pos.X - Origin.X * Scale.X)), (int)Math.Round((Pos.Y - Origin.Y * Scale.Y)), (int)Math.Round((Size.X * Scale.X)), (int)Math.Round((Size.Y * Scale.Y)));
            }
        }
        public Vector2 Scale { get; internal set; }
        public object Tag { get; set; }
        public GameObject(Vector2 pos, Vector2 size, Vector2 scale, Vector2 origin)
        {
            Pos = pos;
            Size = size;
            Scale = scale;
            Origin = origin;
            IsVisable = true;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
