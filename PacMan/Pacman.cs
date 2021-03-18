using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

using System.Text;
using static PacMan.Enum;

namespace PacMan
{
    public class Pacman : Sprite
    {
        private float speed;
        private ScreenManager screenManager;
        private InputManager inputManager;
        private Directions currentDirection;
        public Pacman(Texture2D tex, Color tint, Vector2 pos, Vector2 size, float speedPerSec, ScreenManager screenManager, InputManager inputManager)
            : base(tex, tint, pos, size)
        {
            speed = speedPerSec / 125 * 2;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
            currentDirection = Directions.None;
        }


        public override void Update(GameTime gameTime)
        {
            Keys[] currentKeys = inputManager.KeyboardState.GetPressedKeys();
            if (currentKeys.Length > 0)
            {
                currentDirection = screenManager.Settings.DirectionDictionary[currentKeys[0]];
            }
            switch(currentDirection)
            {
                case Directions.Up:
                    Pos.Y -= speed;
                    break;

                case Directions.Down:
                    Pos.Y += speed;
                    break;

                case Directions.Left:
                    Pos.X -= speed;
                    break;

                case Directions.Right:
                    Pos.X += speed;
                    break;

                default:
                    break;
            }
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{

        //}
    }
}
