using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

using System.Text;
namespace PacMan
{
    public class Pacman : AnimationSprite
    {
        Tile[,] grid;

        public float speed;
        public int iterationsPerUpdate;
        public ScreenManager ScreenManager;
        private InputManager inputManager;
        public Directions CurrentDirection { get; set; }
        public Directions PreviousDirection { get; set; }

        public Directions NextDirection { get; set; }

        private Vector2 previousPos;
        Point posInGrid;
        private bool loadingTeleport;

        public Tile CurrentTile => grid[posInGrid.Y, posInGrid.X];
        public override Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)(Pos.X - Frames[currentIndex].Origin.X * Frames[currentIndex].Scale.X * Scale.X), (int)(Pos.Y - Frames[currentIndex].Origin.Y * Frames[currentIndex].Scale.Y * Scale.Y), (int)(Frames[currentIndex].HitBox.Width * Frames[currentIndex].Scale.X * Scale.X), (int)(Frames[currentIndex].HitBox.Height * Frames[currentIndex].Scale.Y * Scale.Y));
            }
        }

        public Pacman(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames, float speedPerUpdate, ScreenManager screenManager, InputManager inputManager, Tile[,] grid)
            : base(tex, tint, pos, scale, frames, timeBetweenFrames)
        {
            this.grid = grid;

            speed = speedPerUpdate;
            this.ScreenManager = screenManager;
            this.inputManager = inputManager;
            CurrentDirection = Directions.None;
            PreviousDirection = Directions.None;
            posInGrid = GameScreen.PositionToTile(Pos, grid).PositionInGrid;
            previousPos = new Vector2();
            loadingTeleport = false;
        }


        public override void Update(GameTime gameTime)
        {

            Keys[] currentKeys = inputManager.KeyboardState.GetPressedKeys();
            int index = 0;
            if (currentKeys.Length > 0)
            {
                bool shouldScan = true;
                while (!ScreenManager.Settings.DirectionDictionary.ContainsKey(currentKeys[index]))
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
                    NextDirection = ScreenManager.Settings.DirectionDictionary[currentKeys[index]];
                }
            }

            bool goingSamePlane = GoingSamePlane(CurrentDirection, PreviousDirection);
            if (goingSamePlane || IsOnTile(HitBox))
            {
                if (!loadingTeleport && CanMove(NextDirection))
                {
                    CurrentDirection = NextDirection;
                }
                PreviousDirection = CurrentDirection;
                posInGrid = GameScreen.PositionToTile(Pos, grid).PositionInGrid;
                previousPos = Pos;

                Vector2 padding = new Vector2(1, 1);
                //padding = Vector2.Zero;

                loadingTeleport = false;
                switch (CurrentDirection)
                {
                    case Directions.Up:
                        if (posInGrid.Y == 0 && CurrentTile.TileType == TileTypes.Teleport)
                        {
                            loadingTeleport = true;

                            if (IsOnTile(HitBox))
                            {
                                if (!ScreenManager.Settings.TeleportDictionary.ContainsKey(CurrentTile))
                                {
                                    CurrentTile.IsObstacle = false;
                                    Tile key = CurrentTile;
                                    posInGrid.Y = grid.GetLength(1) - 1;
                                    while (CurrentTile.IsObstacle)
                                    {
                                        posInGrid.Y--;
                                    }
                                    Pos.Y = CurrentTile.Pos.Y + HitBox.Height / 2;
                                    ScreenManager.Settings.TeleportDictionary.Add(key, CurrentTile);
                                    key.Neighbors.Add(CurrentTile);
                                }
                                else
                                {
                                    Pos.Y = ScreenManager.Settings.TeleportDictionary[CurrentTile].Pos.Y + HitBox.Height / 2;
                                    //////In this case part the currentTile will be incorrect until the next update because I am teleporting the pacman
                                }
                            }
                        }

                        if (!loadingTeleport && GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y - 1)].TileType == TileTypes.Wall)
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
                        if (posInGrid.Y == 18 && CurrentTile.TileType == TileTypes.Teleport)
                        {
                            loadingTeleport = true;

                            if (IsOnTile(HitBox))
                            {
                                if (!ScreenManager.Settings.TeleportDictionary.ContainsKey(CurrentTile))
                                {
                                    CurrentTile.IsObstacle = false;
                                    Tile key = CurrentTile;
                                    posInGrid.Y = 0;
                                    while (CurrentTile.IsObstacle)
                                    {
                                        posInGrid.Y++;
                                    }
                                    Pos.Y = CurrentTile.Pos.Y + HitBox.Height / 2;
                                    ScreenManager.Settings.TeleportDictionary.Add(key, CurrentTile);
                                    key.Neighbors.Add(CurrentTile);
                                }
                                else
                                {
                                    Pos.Y = ScreenManager.Settings.TeleportDictionary[CurrentTile].Pos.Y + HitBox.Height / 2;
                                }
                            }
                        }
                        if (!loadingTeleport && GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y + 1)].TileType == TileTypes.Wall)
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
                        if (posInGrid.X == 0 & CurrentTile.TileType == TileTypes.Teleport)
                        {
                            loadingTeleport = true;


                            if (IsOnTile(HitBox))
                            {
                                if (!ScreenManager.Settings.TeleportDictionary.ContainsKey(CurrentTile))
                                {
                                    CurrentTile.IsObstacle = false;
                                    Tile key = CurrentTile;
                                    posInGrid.X = grid.GetLength(0) - 1;
                                    while (CurrentTile.IsObstacle)
                                    {
                                        posInGrid.X--;
                                    }
                                    Pos.X = CurrentTile.Pos.X + HitBox.Width / 2;
                                    ScreenManager.Settings.TeleportDictionary.Add(key, CurrentTile);
                                    key.Neighbors.Add(CurrentTile);
                                }
                                else
                                {
                                    Pos.X = ScreenManager.Settings.TeleportDictionary[CurrentTile].Pos.X + HitBox.Width / 2;
                                }
                            }

                        }

                        if (!loadingTeleport && GameScreen.PointToTile[new Point(posInGrid.X - 1, posInGrid.Y)].TileType == TileTypes.Wall)
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
                        if (posInGrid.X == 18 & CurrentTile.TileType == TileTypes.Teleport)
                        {
                            loadingTeleport = true;

                            if (IsOnTile(HitBox))
                            {
                                if (!ScreenManager.Settings.TeleportDictionary.ContainsKey(CurrentTile))
                                {
                                    CurrentTile.IsObstacle = false;
                                    Tile key = CurrentTile;
                                    posInGrid.X = 0;
                                    while (CurrentTile.IsObstacle)
                                    {
                                        posInGrid.X++;
                                    }
                                    Pos.X = CurrentTile.Pos.X + HitBox.Width / 2;
                                    ScreenManager.Settings.TeleportDictionary.Add(key, CurrentTile);
                                    key.Neighbors.Add(CurrentTile);
                                }
                                else
                                {
                                    Pos.X = ScreenManager.Settings.TeleportDictionary[CurrentTile].Pos.X + HitBox.Width / 2;
                                }
                            }
                        }

                        if (!loadingTeleport && GameScreen.PointToTile[new Point(posInGrid.X + 1, posInGrid.Y)].TileType == TileTypes.Wall)
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
            if (Pos.X - HitBox.Width / 2 <= ScreenManager.CurrentScreen.Bounds.Left)
            {
                returnValue = true;
                Pos.X = (int)(HitBox.Width / 2 + ScreenManager.CurrentScreen.Bounds.Left);
            }
            else if (Pos.X + HitBox.Width / 2 >= ScreenManager.CurrentScreen.Bounds.Right)
            {
                returnValue = true;
                Pos.X = (int)(ScreenManager.CurrentScreen.Bounds.Right - HitBox.Width / 2);
            }
            else if (Pos.Y - HitBox.Height / 2 <= ScreenManager.CurrentScreen.Bounds.Top)
            {
                returnValue = true;
                Pos.Y = (int)(HitBox.Height / 2 + ScreenManager.CurrentScreen.Bounds.Top);
            }
            else if (Pos.Y + HitBox.Height / 2 >= ScreenManager.CurrentScreen.Bounds.Bottom)
            {
                returnValue = true;
                Pos.Y = (int)(ScreenManager.CurrentScreen.Bounds.Bottom - HitBox.Height / 2);
            }

            return returnValue;
        }

        private bool IsOnTile(Rectangle hitBox)
        {
            Vector2 size = new Vector2(hitBox.Width, hitBox.Height);
            return GameScreen.PositionToTile(new Vector2(hitBox.X, hitBox.Y + hitBox.Height), grid).PositionInGrid.Equals(GameScreen.PositionToTile(new Vector2(hitBox.X + hitBox.Width, hitBox.Y), grid).PositionInGrid);
        }

        private bool GoingSamePlane(Directions currentDirection, Directions previousDirection)
        {
            if (currentDirection == Directions.Up || currentDirection == Directions.Down)
            {
                return previousDirection == Directions.Up || previousDirection == Directions.Down;
            }
            else if (currentDirection == Directions.None)
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
            if (!IsOnTile(HitBox))
            {
                return false;
            }

            Point pacPoint = GameScreen.PositionToTile(Pos, grid).PositionInGrid;
            switch (nextDirection)
            {
                case Directions.Up:
                    return GameScreen.PointToTile[new Point(pacPoint.X, pacPoint.Y - 1)].TileType != TileTypes.Wall;

                case Directions.Down:
                    return GameScreen.PointToTile[new Point(pacPoint.X, pacPoint.Y + 1)].TileType != TileTypes.Wall;

                case Directions.Right:
                    return GameScreen.PointToTile[new Point(pacPoint.X + 1, pacPoint.Y)].TileType != TileTypes.Wall;

                case Directions.Left:
                    return GameScreen.PointToTile[new Point(pacPoint.X - 1, pacPoint.Y)].TileType != TileTypes.Wall;

                default:
                    return true;
            }
        }
    }
}
