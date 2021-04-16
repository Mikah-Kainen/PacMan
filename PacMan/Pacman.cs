using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

using System.Text;
using static PacMan.Enum;

namespace PacMan
{
    public class Pacman : AnimationSprite
    {
        public float speed;
        public int iterationsPerUpdate;
        private ScreenManager screenManager;
        private InputManager inputManager;
        public Directions CurrentDirection { get; set; }
        public Vector2[] PreviousPositions { get; set; }

        public override Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)(Pos.X - Frames[1].HitBox.Width * Scale.X / 2), (int)(Pos.Y - Frames[1].HitBox.Height * Scale.Y / 2), (int)(Frames[1].HitBox.Width * Scale.X), (int)(Frames[1].HitBox.Height * Scale.Y));
            }
        }

        public Pacman(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames, float speedPerUpdate, int iterationsPerUpdate, ScreenManager screenManager, InputManager inputManager)
            : base(tex, tint, pos, scale, frames, timeBetweenFrames)
        {
            speed = speedPerUpdate;
            this.iterationsPerUpdate = iterationsPerUpdate;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
            CurrentDirection = Directions.None;
            PreviousPositions = new Vector2[8];

        }

        public Pacman(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames, int iterationsPerUpdate, ScreenManager screenManager, InputManager inputManager)
    : base(tex, tint, pos, scale, frames, timeBetweenFrames)
        {
            speed = 1;
            this.iterationsPerUpdate = iterationsPerUpdate;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
            CurrentDirection = Directions.None;
            PreviousPositions = new Vector2[8];

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(Tex, Pos, base.CurrentFrame.HitBox, Tint, Rotation, base.CurrentFrame.Origin, Scale, SpriteEffects.None, 0f);
        //}

        public bool HitBorder()
        {
            bool returnValue = false;
            if (Pos.X - base.CurrentFrame.HitBox.Width * Scale.X / 2 <= screenManager.CurrentScreen.Bounds.Left)
            {
                returnValue = true;
                Pos.X = (int)(base.CurrentFrame.HitBox.Width * Scale.X / 2 + screenManager.CurrentScreen.Bounds.Left);
            }
            else if(Pos.X + base.CurrentFrame.HitBox.Width * Scale.X / 2 >= screenManager.CurrentScreen.Bounds.Right)
            {
                returnValue = true;
                Pos.X = (int)(screenManager.CurrentScreen.Bounds.Right - base.CurrentFrame.HitBox.Width * Scale.X / 2);
            }
            else if (Pos.Y - base.CurrentFrame.HitBox.Height * Scale.Y / 2 <= screenManager.CurrentScreen.Bounds.Top)
            {
                returnValue = true;
                Pos.Y = (int)(base.CurrentFrame.HitBox.Height * Scale.Y / 2 + screenManager.CurrentScreen.Bounds.Top);
            }
            else if (Pos.Y + base.CurrentFrame.HitBox.Height * Scale.Y / 2 >= screenManager.CurrentScreen.Bounds.Bottom)
            {
                returnValue = true;
                Pos.Y = (int)(screenManager.CurrentScreen.Bounds.Bottom - base.CurrentFrame.HitBox.Height * Scale.Y / 2);
            }

            return returnValue;
        }
    }
}
