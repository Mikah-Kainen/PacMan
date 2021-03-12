using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class GameObject
    {
        public Vector2 Pos { get; private set; }
        public Vector2 Size { get; private set; }

        public Rectangle HitBox => new Rectangle((int)Pos.X, (int)Pos.Y, (int)Size.X, (int)Size.Y);

        public GameObject(Vector2 pos, Vector2 size)
        {
            Pos = pos;
            Size = size;
        }
    }
}
