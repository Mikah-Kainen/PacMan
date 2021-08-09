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
        public float SpeedPerUpdate;
        public Stack<Tile> Path { get; set; }
        public Tile PreviousTile { get; set; }
        public Tile CurrentTile { get; set; }
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
            this.SpeedPerUpdate = speedPerUpdate;
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

            //if(watch.ElapsedMilliseconds > 1500/(speedPerUpdate + 1))
            //{
            //    if(hashSet.Count == 1)
            //    {
            //        PreviousTile = null;
            //    }

            //    watch.Restart();
            //    hashSet.Clear();
            //}


            if (Path == null || Path.Count == 0)
            {
                CurrentDirection = Directions.None;
            }
            else
            {
                Vector2 targetPos = Path.Peek().Pos;
                if (targetPos == Pos)
                {
                    Path.Pop();
                }
                if (targetPos.Y < HitBox.Y)
                {
                    CurrentDirection = Directions.Up;
                }
                else if (targetPos.Y > HitBox.Y)
                {
                    CurrentDirection = Directions.Down;
                }
                else if (targetPos.X > HitBox.X)
                {
                    CurrentDirection = Directions.Right;
                }
                else if (targetPos.X < HitBox.X)
                {
                    CurrentDirection = Directions.Left;
                }

            }

            switch (CurrentDirection)
            {
                case Directions.Up:
                    if (CurrentTile.PositionInGrid.Y == 0 && CurrentTile.TileType == TileTypes.Teleport)
                    {
                        CurrentTile = ScreenManager.Settings.TeleportDictionary[CurrentTile];
                        Pos.Y = CurrentTile.Pos.Y + HitBox.Height / 2 + 2;
                    }
                    Pos.Y -= SpeedPerUpdate;
                    currentIndex = 0;
                    break;

                case Directions.Down:
                    if (CurrentTile.PositionInGrid.Y == 18 && GameScreen.PositionToTile(Pos, grid).TileType == TileTypes.Teleport)
                    {
                        CurrentTile = ScreenManager.Settings.TeleportDictionary[CurrentTile];
                        Pos.Y = CurrentTile.Pos.Y + HitBox.Height / 2 - 2;
                    }
                    Pos.Y += SpeedPerUpdate;
                    currentIndex = 1;
                    break;

                case Directions.Right:
                    if (CurrentTile.PositionInGrid.X == 18 & GameScreen.PositionToTile(Pos, grid).TileType == TileTypes.Teleport)
                    {
                        CurrentTile = ScreenManager.Settings.TeleportDictionary[CurrentTile];
                        Pos.X = CurrentTile.Pos.X + HitBox.Width / 2 - 2;
                    }
                    Pos.X += SpeedPerUpdate;
                    currentIndex = 2;
                    break;

                case Directions.Left:
                    if (CurrentTile.PositionInGrid.X == 0 & GameScreen.PositionToTile(Pos, grid).TileType == TileTypes.Teleport)
                    {
                        CurrentTile = ScreenManager.Settings.TeleportDictionary[CurrentTile];
                        Pos.X = CurrentTile.Pos.X + HitBox.Width / 2 + 2;
                    }
                    Pos.X -= SpeedPerUpdate;
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
