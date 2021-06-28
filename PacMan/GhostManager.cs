using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PacMan.TraversalStuff;

using System;
using System.Collections.Generic;
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
            StayHome,
            ChasePacman,
            RunAway,
        };

        public enum Ghosts
        {
            Red = 0,
            Blue = 1,
            Orange = 2,
            Pink = 3,
        };

        public Dictionary<Ghosts, Action<Vector2>> PathCalculation;

        Texture2D[] ghostTextures;
        List<Ghost> ghosts;
        List<AnimationFrame>[] frames;
        Tile[,] grid;
        Pacman pacman;
        List<GhostStates> ghostStates;

        public Dictionary<(Texture2D, List<AnimationFrame>), Func<Vector2, Vector2>> targetPosDictionary { get; private set; }

        /// <summary>
        /// //Maybe do some cool thing where the path a ghost just traveled on has a weight of 1 million so that it won't crash every time it gets stuck in a corner
        /// </summary>
        /// <param name="ghosts"></param>
        /// <param name="specialGhostTex"></param>
        /// <param name="specialGhostFrames"></param>
        /// <param name="grid"></param>
        /// <param name="pacman"></param>
        public GhostManager(List<Ghost> ghosts, Texture2D specialGhostTex, List<AnimationFrame> specialGhostFrames, Tile[,] grid, ref Pacman pacman)
        {
            ghostTextures = new Texture2D[ghosts.Count];
            frames = new List<AnimationFrame>[ghosts.Count];
            for(int i = 0; i < ghostTextures.Length; i ++)
            {
                ghostTextures[i] = ghosts[i].Tex;
                frames[i] = ghosts[i].Frames;
            }

            this.ghosts = ghosts;
            this.grid = grid;
            this.pacman = pacman;
            ghostStates = new List<GhostStates>();
            for(int i = 0; i < ghosts.Count; i ++)
            {
                ghostStates.Add(GhostStates.StayHome);
            }
            ghostStates[(int)Ghosts.Red] = GhostStates.ChasePacman;

            PathCalculation = new Dictionary<Ghosts, Action<Vector2>>()
            {
                [Ghosts.Red] = SetRedGhostPath,
            };
        }


        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < ghosts.Count; i++)
            {
                if (IsOnTile(ghosts[i].Pos, ghosts[i].HitBox))
                {
                    switch (ghostStates[i])
                    {
                        case GhostStates.StayHome:
                            PathCalculation[(Ghosts)i](ghosts[i].Pos);
                            break;

                        case GhostStates.ChasePacman:
                            //needs helper function for the other ghosts since pacman.Pos is not the target pos for every ghost
                            PathCalculation[(Ghosts)i](pacman.Pos);
                            break;

                        case GhostStates.RunAway:
                            //inplement this pls
                            PathCalculation[(Ghosts)i](ghosts[i].Corner);
                            break;
                    }
                }
            }

            GameScreen.screenTint.IsVisable = false;
            foreach (Ghost ghost in ghosts)
            {
                if (pacman.HitBox.Intersects(ghost.HitBox))
                {
                    GameScreen.screenTint.IsVisable = true;
                }
            }

            foreach (Ghost ghost in ghosts)
            {
                ghost.Update(gameTime);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ghost ghost in ghosts)
            {
                ghost.Draw(spriteBatch);
            }
        }

        public void SwitchMode()
        {
            isChasing = !isChasing;
            if (isChasing)
            {
                for (int i = 0; i < ghosts.Count; i++)
                {
                    ghosts[i].Tex = ghostTextures[i];
                    ghosts[i].Frames = frames[i];
                }
            }
            else
            {
                for (int i = 0; i < ghosts.Count; i++)
                {
                    ghosts[i].Tex = runTex;
                    ghosts[i].Frames = runFrames;
                }
            }
        }

        void SetRedGhostPath(Vector2 targetPos)
        {
            Ghost currentGhost = ghosts[(int)Ghosts.Red];
            currentGhost.Path = Traversals.AStar(PositionToTile(currentGhost.Pos), PositionToTile(targetPos), Heuristic, grid, currentGhost.PreviousTile);
        }


        void SetBlueGhostPath()
        {

        }


        void SetOrangeGhostPath()
        {

        }

        void SetPinkGhostPath()
        {

        }

        private bool IsOnTile(Vector2 middlePos, Rectangle Hitbox)
        {
            Vector2 size = new Vector2(Hitbox.Width, Hitbox.Height);
            return PositionToTile(middlePos + size * 1 / 2).PositionInGrid == PositionToTile(middlePos - size * 1 / 2).PositionInGrid;
        }

        public Tile PositionToTile(Vector2 position)
        {
            return grid[(int)((position.X) / GameScreen.TileSize.X), (int)((position.Y) / GameScreen.TileSize.Y)];
        }

        private int Heuristic(Tile currentTile, Tile targetTile)
        {
            return (int)(Math.Abs(currentTile.PositionInGrid.X - targetTile.PositionInGrid.X) + Math.Abs(currentTile.PositionInGrid.Y - targetTile.PositionInGrid.Y));
        }


    }
}