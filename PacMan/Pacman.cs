﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

using System.Text;
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

        private Vector2 previousPos;
        Point posInGrid;
        Func<Vector2, Tile> getTile;
        public override Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)(Pos.X - Frames[currentIndex].Origin.X * Frames[currentIndex].Scale.X * Scale.X), (int)(Pos.Y - Frames[currentIndex].Origin.Y *Frames[currentIndex].Scale.Y * Scale.Y), (int)(Frames[currentIndex].HitBox.Width * Frames[currentIndex].Scale.X * Scale.X), (int)(Frames[currentIndex].HitBox.Height * Frames[currentIndex].Scale.Y * Scale.Y));
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
            previousPos = new Vector2();
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

            bool goingSamePlane = GoingSamePlane(CurrentDirection, PreviousDirection);

            if (goingSamePlane || IsOnTile(Pos, HitBox))
            {
                if(CanMove(NextDirection))
                {
                    CurrentDirection = NextDirection;
                }
                PreviousDirection = CurrentDirection;
                posInGrid = getTile(Pos).PositionInGrid;
                previousPos = Pos;

                Vector2 padding = new Vector2(1, 1);
                //padding = Vector2.Zero;

                switch (CurrentDirection)
                {
                    case Directions.Up:
                        if(posInGrid.Y == 0 && getTile(Pos).TileType == TileType.Teleport)
                        {
                            Pos.Y = Math.Abs(Pos.Y - screenManager.CurrentScreen.Bounds.Height);
                        }
                        else if(GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y - 1)].TileType == TileType.Wall)
                        {
                            Pos.Y = Math.Max(Pos.Y - speed, (float)(posInGrid.Y) * GameScreen.TileSize.Y + HitBox.Height / 2 + padding.Y);
                        }
                        else
                        {
                            Pos.Y -= speed;
                        }
                        Rotation = 3 * (float)Math.PI / 2;
                        break;

                    case Directions.Down:
                        if(posInGrid.Y == 19)
                        {

                        }
                        else if (GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y + 1)].TileType == TileType.Wall)
                        {
                            Pos.Y = Math.Min(Pos.Y + speed, (float)(posInGrid.Y + 1) * GameScreen.TileSize.Y - HitBox.Height / 2 - padding.X);
                        }
                        else
                        {
                            Pos.Y += speed;
                        }
                        Rotation = (float)Math.PI / 2;
                        break;

                    case Directions.Left:
                        if (posInGrid.X == 0)
                        {

                        }    
                        if (GameScreen.PointToTile[new Point(posInGrid.X - 1, posInGrid.Y)].TileType == TileType.Wall)
                        {
                            Pos.X = Math.Max(Pos.X - speed, (float)(posInGrid.X) * GameScreen.TileSize.X + HitBox.Width / 2 + padding.Y);
                        }
                        else
                        {
                            Pos.X -= speed;
                        }
                        Rotation = (float)Math.PI;
                        break;

                    case Directions.Right:
                        if(posInGrid.X == 19)
                        {

                        }
                        if (GameScreen.PointToTile[new Point(posInGrid.X + 1, posInGrid.Y)].TileType == TileType.Wall)
                        {
                            Pos.X = Math.Min(Pos.X + speed, (float)(posInGrid.X + 1) * GameScreen.TileSize.X - HitBox.Width / 2 - padding.X);
                        }
                        else
                        {
                            Pos.X += speed;
                        }
                        Rotation = 0;
                        break;
                }
                if (previousPos != Pos)
                {
                    base.Update(gameTime);
                }
                else 
                {
                    currentIndex = 1;
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
            if (Pos.X - base.CurrentFrame.HitBox.Width * Scale.X * Frames[currentIndex].Scale.X / 2 <= screenManager.CurrentScreen.Bounds.Left)
            {
                returnValue = true;
                Pos.X = (int)(base.CurrentFrame.HitBox.Width * Scale.X * Frames[currentIndex].Scale.X / 2 + screenManager.CurrentScreen.Bounds.Left);
            }
            else if (Pos.X + base.CurrentFrame.HitBox.Width * Scale.X * Frames[currentIndex].Scale.X / 2 >= screenManager.CurrentScreen.Bounds.Right)
            {
                returnValue = true;
                Pos.X = (int)(screenManager.CurrentScreen.Bounds.Right - base.CurrentFrame.HitBox.Width * Scale.X * Frames[currentIndex].Scale.X / 2);
            }
            else if (Pos.Y - base.CurrentFrame.HitBox.Height * Scale.Y * Frames[currentIndex].Scale.Y / 2 <= screenManager.CurrentScreen.Bounds.Top)
            {
                returnValue = true;
                Pos.Y = (int)(base.CurrentFrame.HitBox.Height * Scale.Y * Frames[currentIndex].Scale.Y / 2 + screenManager.CurrentScreen.Bounds.Top);
            }
            else if (Pos.Y + base.CurrentFrame.HitBox.Height * Scale.Y * Frames[currentIndex].Scale.Y / 2 >= screenManager.CurrentScreen.Bounds.Bottom)
            {
                returnValue = true;
                Pos.Y = (int)(screenManager.CurrentScreen.Bounds.Bottom - base.CurrentFrame.HitBox.Height * Scale.Y * Frames[currentIndex].Scale.Y / 2);
            }

            return returnValue;
        }

        private bool IsOnTile(Vector2 middlePos, Rectangle hitBox)
        {
            Vector2 size = new Vector2(hitBox.Width, hitBox.Height);
            return getTile(middlePos + size * 1 / 2).PositionInGrid == getTile(middlePos - size * 1 / 2).PositionInGrid;
        }

        private bool GoingSamePlane(Directions currentDirection, Directions previousDirection)
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

        private bool CanMove(Directions nextDirection)
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
