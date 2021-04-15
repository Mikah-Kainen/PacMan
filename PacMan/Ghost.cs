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
        public Stack<Tile> path;
        public Directions CurrentDirection { get; set; }


        private TimeSpan timeBetweenUpdates;
        int updateCount;


        public Ghost(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, float speedPerUpdate, int iterationsPerUpdate, ScreenManager screenManager, InputManager inputManager, TimeSpan timeBetweenUpdates)
            :base(tex, tint, pos, scale, frames, TimeSpan.FromMilliseconds(100))
        {
            this.speedPerUpdate = speedPerUpdate;
            this.iterationsPerUpdate = iterationsPerUpdate;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
            CurrentDirection = Directions.None;

            this.timeBetweenUpdates = timeBetweenUpdates;
            updateCount = 0;
        }


        public override void Update(GameTime gameTime)
        {
            /////////////////////////////////
            ////////////////////////////////It could be a big problem if the ghost ever gets to his target tile and has nowhere to go but I dont think that will ever happen
            /////////////////////////////////
            if (path == null || path.Count == 0)
            {
                CurrentDirection = Directions.None;
            }
            else if(updateCount * 16 > timeBetweenUpdates.TotalMilliseconds)
            {
                Pos = path.Pop().Pos;
                updateCount = 0;
            }
            updateCount++;
            //else
            //{
            //    if (path.Peek().Pos == Pos)
            //    {
            //        path.Pop();
            //    }
            //    if (path.Peek().Pos.X > Pos.X)
            //    {
            //        CurrentDirection = Directions.Right;
            //    }
            //    else if (path.Peek().Pos.X < Pos.X)
            //    {
            //        CurrentDirection = Directions.Left;
            //    }
            //    else if (path.Peek().Pos.Y < Pos.Y)
            //    {
            //        CurrentDirection = Directions.Up;
            //    }
            //    else if (path.Peek().Pos.Y > Pos.Y)
            //    {
            //        CurrentDirection = Directions.Down;
            //    }
            //}

            //for (int i = 0; i < iterationsPerUpdate; i ++)
            //{
            //    switch (CurrentDirection)
            //    {
            //        case Directions.Up:
            //            Pos.Y -= speedPerUpdate;
            //            currentIndex = 0;
            //            break;

            //        case Directions.Down:
            //            Pos.Y += speedPerUpdate;
            //            currentIndex = 1;
            //            break;

            //        case Directions.Right:
            //            Pos.X += speedPerUpdate;
            //            currentIndex = 2;
            //            break;

            //        case Directions.Left:
            //            Pos.X -= speedPerUpdate;
            //            currentIndex = 3;
            //            break;

            //        default:

            //            break;
            //    }
            //}
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
