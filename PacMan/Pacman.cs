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
        public Directions PreviousDirection { get; set; }

        public Directions NextDirection { get; set; }
        Point posInGrid;
        Func<Vector2, Tile> getTile;
        public override Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)(Pos.X - Frames[1].HitBox.Width * Scale.X / 2), (int)(Pos.Y - Frames[1].HitBox.Height * Scale.Y / 2), (int)(Frames[1].HitBox.Width * Scale.X), (int)(Frames[1].HitBox.Height * Scale.Y));
            }
        }

        public Pacman(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames, float speedPerUpdate, ScreenManager screenManager, InputManager inputManager, Func<Vector2, Tile> getTile)
            : base(tex, tint, pos, scale, frames, timeBetweenFrames)
        {
            speed = speedPerUpdate;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
            CurrentDirection = Directions.None;
            PreviousDirection = Directions.None;
            this.getTile = getTile;
            posInGrid = getTile(Pos).PositionInGrid;
        }

        public Pacman(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames, int iterationsPerUpdate, ScreenManager screenManager, InputManager inputManager)
    : base(tex, tint, pos, scale, frames, timeBetweenFrames)
        {
            speed = 1;
            this.iterationsPerUpdate = iterationsPerUpdate;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
            CurrentDirection = Directions.None;
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
                    NextDirection = screenManager.Settings.DirectionDictionary[currentKeys[index]];
                }
            }

            //x = Math.Min(currentCell.X - currentCell.Width / 2, x)
            //LeftSide: x = Math.Min(currentCell.X - currentCell.Width / 2, x + speed)
            //RightSide: x = Math.Max(currentCell.X - currentCell.Width / 2, x + speed)
            //LeftSide: y = Math.Min(currentCell.X - currentCell.Width / 2, x + speed)
            //RightSide: y = Math.Max(currentCell.X - currentCell.Width / 2, x + speed)

            bool goingSamePlane = GoingSamePlane(CurrentDirection, PreviousDirection);

            if (goingSamePlane || IsOnTile(Pos, HitBox))
            {
                if(CanMove(NextDirection))
                {
                    CurrentDirection = NextDirection;
                }
                PreviousDirection = CurrentDirection;
                posInGrid = getTile(Pos).PositionInGrid;
                switch (CurrentDirection)
                {
                    case Directions.Up:
                        if(GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y - 1)].TileType == TileType.Wall)
                        {
                            Pos.Y = Math.Max(Pos.Y - speed, (float)(posInGrid.Y) * GameScreen.TileSize.Y + HitBox.Height / 2 + 5);
                        }
                        else
                        {
                            Pos.Y -= speed;
                        }
                        Rotation = 3 * (float)Math.PI / 2;
                        break;

                    case Directions.Down:
                        if (GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y + 1)].TileType == TileType.Wall)
                        {
                            Pos.Y = Math.Min(Pos.Y + speed, (float)(posInGrid.Y + 1) * GameScreen.TileSize.Y - HitBox.Height / 2 - 3);
                        }
                        else
                        {
                            Pos.Y += speed;
                        }
                        Rotation = (float)Math.PI / 2;
                        break;

                    case Directions.Left:
                        if (GameScreen.PointToTile[new Point(posInGrid.X - 1, posInGrid.Y)].TileType == TileType.Wall)
                        {
                            Pos.X = Math.Max(Pos.X - speed, (float)(posInGrid.X) * GameScreen.TileSize.X + HitBox.Width / 2 + 5);
                        }
                        else
                        {
                            Pos.X -= speed;
                        }
                        Rotation = (float)Math.PI;
                        break;

                    case Directions.Right:
                        if (GameScreen.PointToTile[new Point(posInGrid.X + 1, posInGrid.Y)].TileType == TileType.Wall)
                        {
                            Pos.X = Math.Min(Pos.X + speed, (float)(posInGrid.X + 1) * GameScreen.TileSize.X - HitBox.Width / 2 - 3);
                        }
                        else
                        {
                            Pos.X += speed;
                        }
                        Rotation = 0;
                        break;

                    default:
                        currentIndex = 1;
                        break;
                }
            }
            if (HitBorder())
            {
                CurrentDirection = Directions.None;
            }
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
            else if (Pos.X + base.CurrentFrame.HitBox.Width * Scale.X / 2 >= screenManager.CurrentScreen.Bounds.Right)
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

        private bool IsOnTile(Vector2 middlePos, Rectangle hitBox)
        {
            Vector2 size = new Vector2(hitBox.Width, hitBox.Height);
            return getTile(middlePos + size * 1 / 2).PositionInGrid == getTile(middlePos - size * 1 / 2).PositionInGrid;
        }

        private bool GoingSamePlane(Enum.Directions currentDirection, Enum.Directions previousDirection)
        {
            if(currentDirection == Directions.Up || currentDirection == Directions.Down)
            {
                return previousDirection == Directions.Up || previousDirection == Directions.Down;
            }
            else if(currentDirection == Directions.None)
            {
                return false;
            }
            else
            {
                return previousDirection == Directions.Right || previousDirection == Directions.Left;
            }
        }

        private bool CanMove(Enum.Directions nextDirection)
        {
            if(!IsOnTile(Pos, HitBox))
            {
                return false;
            }

            Point pacPoint = getTile(Pos).PositionInGrid;
            switch(nextDirection)
            {
                case Directions.Up:
                    return GameScreen.PointToTile[new Point(pacPoint.X, pacPoint.Y - 1)].TileType == TileType.Background;

                case Directions.Down:
                    return GameScreen.PointToTile[new Point(pacPoint.X, pacPoint.Y + 1)].TileType == TileType.Background;

                case Directions.Right:
                    return GameScreen.PointToTile[new Point(pacPoint.X + 1, pacPoint.Y)].TileType == TileType.Background;

                case Directions.Left:
                    return GameScreen.PointToTile[new Point(pacPoint.X - 1, pacPoint.Y)].TileType == TileType.Background;

                default:
                    return true;
            }
        }
    }
}
