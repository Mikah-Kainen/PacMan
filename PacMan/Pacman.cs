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
        private float speed;
        private int iterationsPerUpdate;
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

            set { }
        }

        public Pacman(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames, float speedPerUpdate, int iterationsPerUpdate, ScreenManager screenManager, InputManager inputManager)
            : base(tex, tint, pos, scale, frames, timeBetweenFrames)
        {
            speed = speedPerUpdate;
            this.iterationsPerUpdate = iterationsPerUpdate;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
            CurrentDirection = Directions.None;
            base.isMiddleOrigin = true;
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
            base.isMiddleOrigin = true;
            PreviousPositions = new Vector2[8];

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Keys[] currentKeys = inputManager.KeyboardState.GetPressedKeys();
            int index = 0;
            if (currentKeys.Length > 0)
            {
                bool shouldScan = true;
                while (!screenManager.Settings.DirectionDictionary.ContainsKey(currentKeys[index]))
                {
                    index++;
                    if (index >= currentKeys.Length)
                    {
                        shouldScan = false;
                        break;
                    }
                }
                if (shouldScan)
                {
                    CurrentDirection = screenManager.Settings.DirectionDictionary[currentKeys[index]];
                }
            }
            for (int x = 0; x < iterationsPerUpdate; x ++)
            {
                for (int i = PreviousPositions.Length - 1; i > 0; i--)
                {
                    PreviousPositions[i] = PreviousPositions[i - 1];
                }
                PreviousPositions[0] = Pos;
                switch (CurrentDirection)
                {
                    case Directions.Up:
                        Pos.Y -= speed;
                        Rotation = 3 * (float)Math.PI / 2;
                        break;

                    case Directions.Down:
                        Pos.Y += speed;
                        Rotation = (float)Math.PI / 2;
                        break;

                    case Directions.Left:
                        Pos.X -= speed;
                        Rotation = (float)Math.PI;
                        break;

                    case Directions.Right:
                        Pos.X += speed;
                        Rotation = 0;
                        break;

                    default:
                        base.currentIndex = 1;
                        break;
                }
                if (HitBorder())
                {
                    CurrentDirection = Directions.None;
                }
            }
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(Tex, Pos, base.CurrentFrame.HitBox, Tint, Rotation, base.CurrentFrame.Origin, Scale, SpriteEffects.None, 0f);
        //}

        private bool HitBorder()
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
