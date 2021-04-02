using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static PacMan.Enum;

namespace PacMan
{
    public class Ghost : AnimationSprite
    {
        private float speedPerUpdate;
        private int iterationsPerUpdate;
        private ScreenManager screenManager;
        private InputManager inputManager;
        public Tile targetTile;
        public Directions CurrentDirection { get; set; }

        public Ghost(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, float speedPerUpdate, int iterationsPerUpdate, ScreenManager screenManager, InputManager inputManager)
            :base(tex, tint, pos, scale, frames, TimeSpan.FromMilliseconds(100))
        {
            this.speedPerUpdate = speedPerUpdate;
            this.iterationsPerUpdate = iterationsPerUpdate;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
            CurrentDirection = Directions.None;
        }


        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            for (int i = 0; i < iterationsPerUpdate; i ++)
            {
                switch (CurrentDirection)
                {
                    case Directions.Up:
                        Pos.Y -= speedPerUpdate;
                        currentIndex = 0;
                        break;

                    case Directions.Down:
                        Pos.Y += speedPerUpdate;
                        currentIndex = 1;
                        break;

                    case Directions.Right:
                        Pos.X += speedPerUpdate;
                        currentIndex = 2;
                        break;

                    case Directions.Left:
                        Pos.X -= speedPerUpdate;
                        currentIndex = 3;
                        break;

                    default:

                        break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }


        private Directions CalculateDirection()
        {
            return Directions.None;
        }
    }
}
