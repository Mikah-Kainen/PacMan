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
        private ScreenManager screenManager;
        private InputManager inputManager;
        private Directions currentDirection;
        private float rotation;

        public Pacman(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames, float speedPerSec, ScreenManager screenManager, InputManager inputManager)
            : base(tex, tint, pos, scale, frames, timeBetweenFrames)
        {
            speed = speedPerSec / 125 * 2;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
            currentDirection = Directions.None;
            rotation = 0;
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
                    currentDirection = screenManager.Settings.DirectionDictionary[currentKeys[index]];
                }
            }
            switch (currentDirection)
            {
                case Directions.Up:
                    Pos.Y -= speed;
                    rotation = 3 * (float)Math.PI / 2;
                    break;

                case Directions.Down:
                    Pos.Y += speed;
                    rotation = (float)Math.PI / 2;
                    break;

                case Directions.Left:
                    Pos.X -= speed;
                    rotation = (float)Math.PI;
                    break;

                case Directions.Right:
                    Pos.X += speed;
                    rotation = 0;
                    break;

                default:
                    base.currentIndex = 1;
                    break;
            }
            if(HitBorder())
            {
                currentDirection = Directions.None;
                base.currentIndex = 1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Tex, Pos, base.CurrentFrame.HitBox, Tint, rotation, base.CurrentFrame.Origin, Scale, SpriteEffects.None, 0f);
        }

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
