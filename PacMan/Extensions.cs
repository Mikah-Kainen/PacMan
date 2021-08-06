using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PacMan.TraversalStuff;

using System;
using System.Collections.Generic;


using System.Text;

namespace PacMan
{
    public static class Extensions
    {

        public static Texture2D CreatePixel(this Color color, GraphicsDevice device)
        {
            Texture2D texture = new Texture2D(device, 1, 1);
            texture.SetData(new[] { color });

            return texture;
        }

        public static Tile CreateTile(this Color color, GraphicsDevice device, TileTypes tileType, Vector2 scale, Point posInGrid)
        {
            return new Tile(color.CreatePixel(device), color, scale, tileType, posInGrid);
        }

        public static Sprite CreateSprite(this Color color, GraphicsDevice device, Vector2 tileSize, Point posInGrid)
        {
            return new Sprite(Color.White.CreatePixel(device), color, new Vector2(posInGrid.X * tileSize.X, posInGrid.Y * tileSize.Y), tileSize, Vector2.Zero);
        }

        public static Tile CreateTile(this (Color color, TileTypes tileType) tuple, GraphicsDevice device, Vector2 tileSize, Point posInGrid)
        {
            return new Tile(Color.White.CreatePixel(device), tuple.color, tileSize, tuple.tileType, posInGrid);
        }

        public static int ToArgb(this Color color)
        {
            int iCol = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
            return iCol;
        }

        public static Color FromArgb(this int argb)
        {
            byte alpha = (byte)(argb >> 24);
            byte red = (byte)(argb >> 16);
            byte green = (byte)(argb >> 8);
            byte blue = (byte)(argb);

            return new Color(red, green, blue, alpha);
        }

        public static AnimationFrame CreateFrame(this Rectangle source, bool isMiddleOrigin, Vector2 scale)
        {
            if (isMiddleOrigin)
            {
                return new AnimationFrame(source, new Vector2(source.Width / 2, source.Height / 2), new Vector2(scale.X / source.Width, scale.Y / source.Height));
            }
            return new AnimationFrame(source, Vector2.Zero, new Vector2(scale.X / source.Width, scale.Y / source.Height));
        }

        public static bool IsPacObstacle(this Tile targetTile)
        {
            return !targetTile.IsObstacle || targetTile.TileType == TileTypes.Teleport;
        }
    }

    public static class GhostExtensions
    {
        private static int Heuristic(Tile currentTile, Tile targetTile)
        {
            return (int)(Math.Abs(currentTile.PositionInGrid.X - targetTile.PositionInGrid.X) + Math.Abs(currentTile.PositionInGrid.Y - targetTile.PositionInGrid.Y));
        }
        public static void SetPath(this Ghost ghost, Vector2 targetPos, Tile[,] grid)
        {
            Tile startingTile = GameScreen.PositionToTile(ghost.Pos, grid);
            Tile targetTile = GameScreen.PositionToTile(targetPos, grid);
            Tile previousTile = ghost.PreviousTile;
            if (ghost.CanMovePrevious(grid))
            {
                previousTile = null;
            }

            ghost.Path = Traversals<Tile>.AStar(startingTile, Traversals<Tile>.FindClosestTarget(targetTile, targetTile, Heuristic, grid, previousTile), Heuristic, grid, previousTile);

        }

        public static bool CanMovePrevious(this Ghost ghost, Tile[,] grid)
        {
            int possiblePaths = 0;
            List<Tile> neighbors = ghost.CurrentTile.Neighbors;
            foreach (Tile tile in neighbors)
            {
                if (!tile.IsObstacle)
                {
                    possiblePaths++;
                }
            }
            return possiblePaths == 1;
        }
    }
}
