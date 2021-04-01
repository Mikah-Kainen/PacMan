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
        public virtual ref Vector2 Pos => ref position;
        public Vector2 Size { get; internal set; }

        public Vector2 Origin;
        public virtual Rectangle HitBox
        {
            get
            {
                //if ()
                //{
                //    return new Rectangle((int)Pos.X, (int)Pos.Y, (int)(Size.X * Scale.X), (int)(Size.Y * Scale.Y));
                //}
                //else
                //{
                    return new Rectangle((int)(Pos.X - Size.X * Scale.X / 2), (int)(Pos.Y - Size.Y * Scale.Y / 2), (int)(Size.X * Scale.X), (int)(Size.Y * Scale.Y));
                //}
            }
        }
        public Vector2 Scale { get; internal set; }
        public GameObject(Vector2 pos, Vector2 size, Vector2 scale, Vector2 origin)
        {
            Pos = pos;
            Size = size;
            Scale = scale;
            Origin = origin;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
