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
        public Vector2[] PreviousPositions { get; set; }

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
            PreviousPositions = new Vector2[2];
            this.getTile = getTile;
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
                    PreviousDirection = CurrentDirection;
                    CurrentDirection = screenManager.Settings.DirectionDictionary[currentKeys[index]];
                }
            }

            //THESE ARE NOT THE RIGHT NUMBERS
            //x = Math.Min(currentCell.X - currentCell.Width / 2, x)
            //LeftSide: x = Math.Min(currentCell.X - currentCell.Width / 2, x + speed)
            //RightSide: x = Math.Max(currentCell.X - currentCell.Width / 2, x + speed)
            //LeftSide: y = Math.Min(currentCell.X - currentCell.Width / 2, x + speed)
            //RightSide: y = Math.Max(currentCell.X - currentCell.Width / 2, x + speed)

            Vector2 deltaDistance = Pos - PreviousPositions[0];
            PreviousPositions[0] = Pos;
            bool goingSameDirection = CurrentDirection == PreviousDirection;
            if (goingSameDirection || IsOnTile(Pos, HitBox))
            {
                Point posInGrid = getTile(Pos).PositionInGrid;
                switch (CurrentDirection)
                {
                    case Directions.Up:
                        if (goingSameDirection || GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y + 1)].TileType == TileType.Background)
                        {
                            Pos.Y -= speed;
                            Rotation = 3 * (float)Math.PI / 2;
                        }
                        break;

                    case Directions.Down:
                        if (goingSameDirection || GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y - 1)].TileType == TileType.Background)
                        {
                            Pos.Y += speed;
                            Rotation = (float)Math.PI / 2;
                        }
                        break;

                    case Directions.Left:
                        if (goingSameDirection || GameScreen.PointToTile[new Point(posInGrid.X - 1, posInGrid.Y)].TileType == TileType.Background)
                        {
                            Pos.X -= speed;
                            Rotation = (float)Math.PI;
                        }
                        break;

                    case Directions.Right:
                        if (goingSameDirection || GameScreen.PointToTile[new Point(posInGrid.X + 1, posInGrid.Y)].TileType == TileType.Background)
                        {
                            Pos.X += speed;
                            Rotation = 0;
                        }
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
    }
}
