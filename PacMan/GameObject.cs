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

        public Rectangle HitBox => new Rectangle((int)Pos.X, (int)Pos.Y, (int)Size.X, (int)Size.Y);

        public GameObject(Vector2 pos, Vector2 size)
        {
            Pos = pos;
            Size = size;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
