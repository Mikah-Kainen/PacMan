using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public abstract class GameObject
    {
        private Vector2 position;
        public ref Vector2 Pos => ref position;
        public Vector2 Size { get; internal set; }
        public Rectangle HitBox => new Rectangle((int)Pos.X, (int)Pos.Y, (int)(Size.X * Scale.X), (int)(Size.Y * Scale.Y));
        public Vector2 Scale { get; internal set; }
        public GameObject(Vector2 pos, Vector2 size, Vector2 scale)
        {
            Pos = pos;
            Size = size;
            Scale = scale;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
