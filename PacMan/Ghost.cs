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
        private float speedPerUpdate;
        public Stack<Tile> Path { get; set; }
        public Tile PreviousTile { get; set; }
        public Tile CurrentTile { get; set; }
        public Corner Corner { get; set; }
        public GhostStates CurrentState { get; set; }
        public Directions CurrentDirection { get; set; }

        Tile[,] grid;

        private Stopwatch watch;
        private HashSet<Tile> hashSet;

        public Ghost(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, float speedPerUpdate, Tile[,] grid)
            : base(tex, tint, pos, scale, frames, TimeSpan.FromMilliseconds(100))
        {
            this.speedPerUpdate = speedPerUpdate;
            CurrentDirection = Directions.None;


            CurrentTile = GameScreen.PositionToTile(pos, grid);
            PreviousTile = GameScreen.PositionToTile(pos, grid);

            //CurrentState = GhostStates.StayHome;
            CurrentState = GhostStates.ChasePacman;

            this.grid = grid;

            watch = new Stopwatch();
            watch.Start();
            hashSet = new HashSet<Tile>();
        }


        public override void Update(GameTime gameTime)
        {
            /////////////////////////////////
            ////////////////////////////////It could be a big problem if the ghost ever gets to his target tile and has nowhere to go but I dont think that will ever happen
            /////////////////////////////////
            hashSet.Add(PreviousTile);
            if(GameScreen.PositionToTile(Pos, grid) != CurrentTile)
            {
                PreviousTile = CurrentTile;
                CurrentTile = GameScreen.PositionToTile(Pos, grid);
            }

            if(watch.ElapsedMilliseconds > 500)
            {
                if(hashSet.Count == 1)
                {
                    PreviousTile = null;
                }

                watch.Restart();
                hashSet.Clear();
            }


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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
