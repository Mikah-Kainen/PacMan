using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static PacMan.GhostManager;

namespace PacMan
{
    public class Ghost : AnimationSprite
    {
        public float speed;
        public Stack<Tile> Path { get; set; }
        public Tile PreviousTile { get; set; }
        public Tile CurrentTile { get; set; }
        Point posInGrid => CurrentTile.PositionInGrid;
        public Corner Corner { get; set; }
        public GhostStates CurrentState { get; set; }
        public Directions CurrentDirection { get; set; }

        Tile[,] grid;

        private Stopwatch watch;
        private HashSet<Tile> hashSet;

        public ScreenManager ScreenManager { get; set; }

        public Ghost(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, float speedPerUpdate, Tile[,] grid, ScreenManager screenManager)
            : base(tex, tint, pos, scale, frames, TimeSpan.FromMilliseconds(100))
        {
            this.speed = speedPerUpdate;
            CurrentDirection = Directions.None;


            CurrentTile = GameScreen.PositionToTile(pos, grid);
            PreviousTile = GameScreen.PositionToTile(pos, grid);

            //CurrentState = GhostStates.StayHome;
            CurrentState = GhostStates.StayHome;

            this.grid = grid;

            watch = new Stopwatch();
            watch.Start();
            hashSet = new HashSet<Tile>();

            this.ScreenManager = screenManager;
        }


        public override void Update(GameTime gameTime)
        {
            hashSet.Add(PreviousTile);
            if(GameScreen.PositionToTile(Pos, grid) != CurrentTile)
            {
                PreviousTile = CurrentTile;
                CurrentTile = GameScreen.PositionToTile(Pos, grid);
            }


            if (Path == null || Path.Count == 0)
            {
                CurrentDirection = Directions.None;
            }
            else
            {
                Tile targetTile = Path.Peek();
                if (targetTile.Pos == Pos)
                {
                    Path.Pop();
                }
                if (targetTile.PositionInGrid.Y < posInGrid.Y)
                {
                    CurrentDirection = Directions.Up;
                }
                else if (targetTile.PositionInGrid.Y > posInGrid.Y)
                {
                    CurrentDirection = Directions.Down;
                }
                else if (targetTile.PositionInGrid.X > posInGrid.X)
                {
                    CurrentDirection = Directions.Right;
                }
                else if (targetTile.PositionInGrid.X < posInGrid.X)
                {
                    CurrentDirection = Directions.Left;
                }

            }

            Vector2 padding = new Vector2(1, 1);
            switch (CurrentDirection)
            {
                case Directions.Up:
                    if (CurrentTile.PositionInGrid.Y == 0 && CurrentTile.TileType == TileTypes.Teleport)
                    {
                        CurrentTile = ScreenManager.Settings.TeleportDictionary[CurrentTile];
                        Pos.Y = CurrentTile.Pos.Y + HitBox.Height / 2 + padding.Y;
                    }
                    else if (GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y - 1)].TileType == TileTypes.Wall)
                    {
                        Pos.Y = Math.Max(Pos.Y - speed, (float)(posInGrid.Y) * GameScreen.TileSize.Y + HitBox.Height / 2 + padding.Y);
                    }
                    else
                    {
                        Pos.Y -= speed;
                    }
                    currentIndex = 0;
                    break;

                case Directions.Down:
                    if (CurrentTile.PositionInGrid.Y == 18 && GameScreen.PositionToTile(Pos, grid).TileType == TileTypes.Teleport)
                    {
                        CurrentTile = ScreenManager.Settings.TeleportDictionary[CurrentTile];
                        Pos.Y = CurrentTile.Pos.Y + HitBox.Height / 2 - padding.Y;
                    }
                    else if (GameScreen.PointToTile[new Point(posInGrid.X, posInGrid.Y + 1)].TileType == TileTypes.Wall)
                    {
                        Pos.Y = Math.Min(Pos.Y + speed, (float)(posInGrid.Y + 1) * GameScreen.TileSize.Y - HitBox.Height / 2 - padding.Y);
                    }
                    else
                    {
                        Pos.Y += speed;
                    }
                    currentIndex = 1;
                    break;

                case Directions.Right:
                    if (CurrentTile.PositionInGrid.X == 18 & GameScreen.PositionToTile(Pos, grid).TileType == TileTypes.Teleport)
                    {
                        CurrentTile = ScreenManager.Settings.TeleportDictionary[CurrentTile];
                        Pos.X = CurrentTile.Pos.X + HitBox.Width / 2 - padding.X;
                    }
                    if (GameScreen.PointToTile[new Point(posInGrid.X + 1, posInGrid.Y)].TileType == TileTypes.Wall)
                    {
                        Pos.X = Math.Min(Pos.X + speed, (float)(posInGrid.X + 1) * GameScreen.TileSize.X - HitBox.Width / 2 - padding.X);
                    }
                    else
                    {
                        Pos.X += speed;
                    }
                    currentIndex = 2;
                    break;

                case Directions.Left:
                    if (CurrentTile.PositionInGrid.X == 0 & GameScreen.PositionToTile(Pos, grid).TileType == TileTypes.Teleport)
                    {
                        CurrentTile = ScreenManager.Settings.TeleportDictionary[CurrentTile];
                        Pos.X = CurrentTile.Pos.X + HitBox.Width / 2 + padding.X;
                    }
                    else if (GameScreen.PointToTile[new Point(posInGrid.X - 1, posInGrid.Y)].TileType == TileTypes.Wall)
                    {
                        Pos.X = Math.Max(Pos.X - speed, (float)(posInGrid.X) * GameScreen.TileSize.X + HitBox.Width / 2 + padding.X);
                    }
                    else
                    {
                        Pos.X -= speed;
                    }
                    currentIndex = 3;
                    break;

                default:

                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
