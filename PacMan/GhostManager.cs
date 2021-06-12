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
        Texture2D[] ghostTextures;
        Ghost[] ghosts;
        List<AnimationFrame>[] frames;
        Texture2D runTex;
        List<AnimationFrame> runFrames;
        Tile[,] grid;
        Pacman pacman;
        bool isChasing;

        public Dictionary<(Texture2D, List<AnimationFrame>), Func<Vector2, Vector2>> targetPosDictionary { get; private set; }

        public GhostManager(Texture2D[] ghostTextures, Ghost[] ghosts, List<AnimationFrame>[] frames, Texture2D runTex, List<AnimationFrame> runFrames, Tile[,] grid, ref Pacman pacman)
        {
            this.ghostTextures = ghostTextures;
            this.ghosts = ghosts;
            this.frames = frames;
            this.runTex = runTex;
            this.runFrames = runFrames;
            this.grid = grid;
            this.pacman = pacman;
            isChasing = true;

            targetPosDictionary = new Dictionary<(Texture2D, List<AnimationFrame>), Func<Vector2, Vector2>>();
            for (int i = 0; i < ghosts.Length; i++)
            {
                //                targetPosDictionary.Add((ghostTextures[i], frames[i]), pacman.Pos);
            }
        }


        public void Update(GameTime gameTime)
        {
            foreach (Ghost ghost in ghosts)
            {
                if (IsOnTile(ghost.Pos, ghost.HitBox))
                {
                    ghosts[0].path = Traversals.AStar(PositionToTile(ghost.Pos), PositionToTile(pacman.Pos), Heuristic, grid, ghost.PreviousTile);
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
                for (int i = 0; i < ghosts.Length; i++)
                {
                    ghosts[i].Tex = ghostTextures[i];
                    ghosts[i].Frames = frames[i];
                }
            }
            else
            {
                for (int i = 0; i < ghosts.Length; i++)
                {
                    ghosts[i].Tex = runTex;
                    ghosts[i].Frames = runFrames;
                }
            }
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