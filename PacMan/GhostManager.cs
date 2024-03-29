﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PacMan.TraversalStuff;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PacMan
{
    public class GhostManager
    {
        public enum FrameIndices
        {
            Runaway1 = 0,
            Runaway2 = 1,
            Fade1 = 2,
            Fade2 = 3,
            EyesRight = 4,
            EyesDown = 5,
            EyesLeft = 6,
            EyesUp = 7,
        };

        public enum GhostStates
        {
            StayHome = 10,
            ChasePacman = 11,
            Scatter = 12,
            Eaten = 13,
            RunAway = 0,
            FadeRun = 1,
        };

        public enum Corner
        {
            TopRight, // red
            TopLeft, // pink
            BottomRight, // blue
            BottomLeft, // orange
        };

        public enum Ghosts
        {
            Red = 0,
            Pink = 1,
            Blue = 2,
            Orange = 3,
        };

        public Dictionary<Ghosts, Func<Vector2>> GetTarget;

        Texture2D[] ghostTextures;
        List<Ghost> ghosts;
        List<AnimationFrame>[] frames;
        Tile[,] grid;
        Pacman pacman;

        Texture2D specialGhostTex;
        List<AnimationFrame> runAwayFrames;
        List<AnimationFrame> fadeRunFrames;
        List<AnimationFrame> eatenFrames;
        public Dictionary<Corner, Tile> CornerToTile { get; private set; }
        public Stopwatch StopWatch;
        public Stopwatch IntervalCounter;
        public Stopwatch ShouldReleaseGhost;
        public List<TimeSpan> Intervals;
        public int CurrentInterval;
        public GhostStates GeneralState { get; set; }

        //0 = red, 1 = pink, 2 = blue, 3 = orange
        public List<Tile> GhostStarts;
        public GhostManager(List<Ghost> ghosts, Texture2D specialGhostTex, List<AnimationFrame> specialGhostFrames, Tile[,] grid, ref Pacman pacman)
        {
            this.specialGhostTex = specialGhostTex;
            runAwayFrames = new List<AnimationFrame>();
            fadeRunFrames = new List<AnimationFrame>();
            eatenFrames = new List<AnimationFrame>();

            for (int i = 0; i < 4; i++)
            {
                runAwayFrames.Add(specialGhostFrames[i]);
            }
            for (int i = 4; i < 8; i++)
            {
                fadeRunFrames.Add(specialGhostFrames[i]);
            }
            //targetOrder: up down right left
            //currentOrder: right down left up
            eatenFrames.Add(specialGhostFrames[8 + 3]);
            eatenFrames.Add(specialGhostFrames[8 + 1]);
            eatenFrames.Add(specialGhostFrames[8 + 0]);
            eatenFrames.Add(specialGhostFrames[8 + 2]);

            this.ghosts = ghosts;
            this.grid = grid;
            this.pacman = pacman;

             GhostStarts = new List<Tile>()
             {
                 //fill this in with the proper ghost start positions
                 grid[8, 8],
                 grid[8, 10],
                 grid[9, 8],
                 grid[9, 10],
             };

            GetTarget = new Dictionary<Ghosts, Func<Vector2>>()
            {
                [Ghosts.Red] = GetRedGhostTarget,
                [Ghosts.Pink] = GetPinkGhostTarget,
                [Ghosts.Blue] = GetBlueGhostTarget,
                [Ghosts.Orange] = GetOrangeGhostTarget,
            };

            CornerToTile = new Dictionary<Corner, Tile>()
            {
                [Corner.TopLeft] = grid[0, 0],
                [Corner.TopRight] = grid[0, grid.GetLength(1) - 1],
                [Corner.BottomLeft] = grid[grid.GetLength(0) - 1, 0],
                [Corner.BottomRight] = grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1],
            };
            StopWatch = new Stopwatch();
            IntervalCounter = new Stopwatch();
            ShouldReleaseGhost = new Stopwatch();

            Intervals = new List<TimeSpan>();
            Intervals.Add(TimeSpan.FromSeconds(7));
            Intervals.Add(TimeSpan.FromSeconds(20));
            Intervals.Add(TimeSpan.FromSeconds(7));
            Intervals.Add(TimeSpan.FromSeconds(20));
            Intervals.Add(TimeSpan.FromSeconds(5));
            Intervals.Add(TimeSpan.FromSeconds(20));
            Intervals.Add(TimeSpan.FromSeconds(5));

            ghostTextures = new Texture2D[ghosts.Count];
            frames = new List<AnimationFrame>[ghosts.Count];
            for (int i = 0; i < ghostTextures.Length; i++)
            {
                ghostTextures[i] = ghosts[i].Tex;
                frames[i] = ghosts[i].Frames;
                ghosts[i].Corner = (Corner)i;
                //Tile startTile = Traversals<Tile>.FindClosestTarget(CornerToTile[(Corner)i], CornerToTile[(Corner)i], GameScreen.Heuristic, grid, null);
            }

            Reset();
        }


        public void Update(GameTime gameTime)
        {
            GameScreen.screenTint.IsVisable = false;
            foreach (Ghost ghost in ghosts)
            {
                if (pacman.HitBox.Intersects(ghost.HitBox))
                {
                    if (ghost.CurrentState == GhostStates.RunAway || ghost.CurrentState == GhostStates.FadeRun || ghost.CurrentState == GhostStates.Eaten)
                    {
                        ghost.CurrentState = GhostStates.Eaten;
                    }
                    else
                    {
                        GameScreen.screenTint.IsVisable = true;
                    }
                }
            }

            foreach (Ghost ghost in ghosts)
            {
                ghost.Update(gameTime);
            }

            if(ShouldReleaseGhost.ElapsedMilliseconds > 3000)
            {
                foreach(Ghost ghost in ghosts)
                {
                    if(ghost.CurrentState == GhostStates.StayHome)
                    {
                        ghost.CurrentState = GhostStates.ChasePacman;
                        ShouldReleaseGhost.Restart();
                        break;
                    }
                }
            }

            if(CurrentInterval < Intervals.Count && IntervalCounter.ElapsedMilliseconds > Intervals[CurrentInterval].TotalMilliseconds)
            {
                IntervalCounter.Restart();
                if(GeneralState == GhostStates.Scatter)
                {
                    GeneralState = GhostStates.ChasePacman;
                }
                else if(GeneralState == GhostStates.ChasePacman)
                {
                    GeneralState = GhostStates.Scatter;
                }
                SwitchMode(GeneralState);
                CurrentInterval++;
            }

            if (GeneralState == GhostStates.RunAway || GeneralState == GhostStates.FadeRun)
            {
                IntervalCounter.Stop();
                if (StopWatch.ElapsedMilliseconds > 6000 && GeneralState == GhostStates.RunAway)
                {
                    SwitchMode(GhostStates.FadeRun);
                }
                if (StopWatch.ElapsedMilliseconds > 1000 && GeneralState == GhostStates.FadeRun)
                {
                    SwitchMode(GhostStates.ChasePacman);
                    IntervalCounter.Start();
                }
            }
            for (int i = 0; i < ghosts.Count; i++)
            {
                if (IsOnTile(ghosts[i].Pos, ghosts[i].HitBox))
                {

                    switch (ghosts[i].CurrentState)
                    {
                        case GhostStates.StayHome:
                            break;

                        case GhostStates.ChasePacman:
                            ghosts[i].Tex = ghostTextures[i];
                            ghosts[i].Frames = frames[i];
                            ghosts[i].SetPath(GetTarget[(Ghosts)i](), grid);
                            break;

                        case GhostStates.Scatter:
                            ghosts[i].Tex = ghostTextures[i];
                            ghosts[i].Frames = frames[i];
                            ghosts[i].SetPath(CornerToTile[ghosts[i].Corner].Pos, grid);
                            break;

                        case GhostStates.Eaten:
                            Tile startTile = GhostStarts[i];
                            ghosts[i].Pos.X = startTile.Pos.X + GameScreen.TileSize.X / 2;
                            ghosts[i].Pos.Y = startTile.Pos.Y + GameScreen.TileSize.Y / 2;
                            ghosts[i].CurrentState = GhostStates.StayHome;
                            ghosts[i].Path = new Stack<Tile>();

                            ghosts[i].Tex = ghostTextures[i];
                            ghosts[i].Frames = frames[i];
                            break;

                        case GhostStates.RunAway:
                            ghosts[i].Tex = specialGhostTex;
                            ghosts[i].Frames = runAwayFrames;

                            Random random = new Random();
                            int targetGhost = random.Next() % 4;

                            ghosts[i].SetPath(ghosts[targetGhost].Pos, grid);
                            break;


                        case GhostStates.FadeRun:
                            ghosts[i].Tex = specialGhostTex;
                            ghosts[i].Frames = fadeRunFrames;
                            //ghosts[i].SetPath(CornerToTile[ghosts[i].Corner].Pos, grid);
                            //////The ghost shakes when it is in this stage because it is shifting from left to right or right to left in the same tile
                            break;
                    }
                }
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ghost ghost in ghosts)
            {
                ghost.Draw(spriteBatch);
            }
        }

        public void SwitchMode(GhostStates newState)
        {
            StopWatch.Restart();
            foreach (Ghost ghost in ghosts)
            {
                switch (ghost.CurrentState)
                {
                    case GhostStates.StayHome:
                        break;

                    case GhostStates.Eaten:
                        break;

                    default:
                        ghost.CurrentState = newState;
                        break;
                }
            }

            GeneralState = newState;
        }

        Vector2 GetRedGhostTarget()
        {
            Vector2 targetPos = pacman.Pos;


            return targetPos;
        }

        Vector2 GetPinkGhostTarget()
        {

            Vector2 targetPos = Vector2.Zero;

            switch (pacman.CurrentDirection)
            {
                case Directions.Up:
                    targetPos = new Vector2(pacman.Pos.X - GameScreen.TileSize.X * 4, pacman.Pos.Y - GameScreen.TileSize.Y * 4);
                    break;

                case Directions.Down:
                    targetPos = new Vector2(pacman.Pos.X, pacman.Pos.Y + GameScreen.TileSize.Y * 4);
                    break;

                case Directions.Left:
                    targetPos = new Vector2(pacman.Pos.X - GameScreen.TileSize.X * 4, pacman.Pos.Y);
                    break;

                case Directions.Right:
                    targetPos = new Vector2(pacman.Pos.X + GameScreen.TileSize.X * 4, pacman.Pos.Y);
                    break;

                case Directions.None:
                    targetPos = new Vector2(pacman.Pos.X + GameScreen.TileSize.X * 4, pacman.Pos.Y);
                    break;
            }

            return targetPos;
        }

        Vector2 GetBlueGhostTarget()
        {
            Point RedGhostPoint = GameScreen.PositionToTile(ghosts[0].Pos, grid).PositionInGrid;
            Point PacPoint = GameScreen.PositionToTile(pacman.Pos, grid).PositionInGrid;
            Point distance = PacPoint - RedGhostPoint;

            Point targetPoint = PacPoint + distance;
            Vector2 returnVal = targetPoint.ToVector2() * GameScreen.TileSize;
            return returnVal;
        }


        Vector2 GetOrangeGhostTarget()
        {
            Vector2 targetPos = pacman.Pos;

            Point OrangeGhostPoint = GameScreen.PositionToTile(ghosts[3].Pos, grid).PositionInGrid;
            Point PacPoint = GameScreen.PositionToTile(pacman.Pos, grid).PositionInGrid;
            Point distance = PacPoint - OrangeGhostPoint;
            if (distance.X * distance.X + distance.Y * distance.Y < 64)
            {
                targetPos = CornerToTile[ghosts[3].Corner].Pos;
            }

            return targetPos;
        }


        private bool IsOnTile(Vector2 middlePos, Rectangle Hitbox)
        {
            Vector2 size = new Vector2(Hitbox.Width, Hitbox.Height);
            return GameScreen.PositionToTile(middlePos + size / 2, grid).PositionInGrid == GameScreen.PositionToTile(middlePos - size * 1 / 2, grid).PositionInGrid;
        }

        public void Reset()
        {
            StopWatch.Restart();
            IntervalCounter.Restart();
            ShouldReleaseGhost.Restart();
            CurrentInterval = 0;

            for (int i = 0; i < ghosts.Count; i ++)
            {
                Tile startTile = GhostStarts[i];
                ghosts[i].Pos.X = startTile.Pos.X + GameScreen.TileSize.X / 2;
                ghosts[i].Pos.Y = startTile.Pos.Y + GameScreen.TileSize.Y / 2;
                ghosts[i].CurrentState = GhostStates.StayHome;
                ghosts[i].Path = new Stack<Tile>();

                ghosts[i].Tex = ghostTextures[i];
                ghosts[i].Frames = frames[i];
            }
            ghosts[0].CurrentState = GhostStates.Scatter;
            GeneralState = GhostStates.Scatter;
        }

    }
}