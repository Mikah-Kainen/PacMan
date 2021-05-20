﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PacMan.TraversalStuff;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacMan
{
    public class GameScreen : Screen
    {
        private Texture2D pixelMap;
        private Pacman pacman;
        private List<Ghost> ghosts;
        private List<Tile> walls;

        public static Vector2 TileSize;
        public static Dictionary<Point, Tile> PointToTile;
        private Tile[,] grid;
        private Rectangle screen => GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds;

        public GameScreen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            base.Load(graphics, content, bounds, screenManager, inputManager);

        }

        public override void Init()
        {
            walls = new List<Tile>();
            ghosts = new List<Ghost>();
            PointToTile = new Dictionary<Point, Tile>();
            pixelMap = ContentManager.Load<Texture2D>("pacmanmap");
            grid = new Tile[pixelMap.Width, pixelMap.Height];


            Color[] pixels = new Color[pixelMap.Width * pixelMap.Height];
            pixelMap.GetData(pixels);

            float xChunk = (int)screen.Width / pixelMap.Width + 1;
            float yChunk = (int)screen.Height / pixelMap.Height + 1;

            TileSize = new Vector2(xChunk, yChunk);

            for (int x = 0; x < pixelMap.Width; x++)
            {
                for (int y = 0; y < pixelMap.Height; y++)
                {

                    int index = CalculateIndex(x, y, pixelMap.Width);
                    Color pixelColor = pixels[index];
                    Point tempPoint = new Point(y, x);

                    if (!PointToTile.ContainsKey(tempPoint))
                    {
                        PointToTile.Add(tempPoint, pixelColor.CreateTile(GraphicsDeviceManager.GraphicsDevice, ScreenManager.Settings.ColorDictionary[pixelColor], TileSize, tempPoint));
                    }
                    Tile temp = PointToTile[tempPoint];

                    grid[y, x] = temp;

                    List<Tile> neighbors = new List<Tile>();

                    temp.Neighbors = GetNeighbors(x, y, pixels);
                    Objects.Add(temp);

                    var tile = Objects[y + x * pixelMap.Width] as Tile;

                    if (tile.TileType == TileType.Wall)
                    {
                        walls.Add(tile);
                    }
                }
            }

            Texture2D pacmansprite = ContentManager.Load<Texture2D>("pacmansprite");

            //What is a good width and height for the pacman?


            // 175 193
            Vector2 pacSize = new Vector2(TileSize.X - 5, TileSize.Y - 5);

            var frameList = new List<AnimationFrame>();
            frameList.Add(new AnimationFrame(new Rectangle(0, 0, 136, 193), new Vector2(68, 96.5f), new Vector2(pacSize.X / 136f, pacSize.Y / 193f)));
            frameList.Add(new AnimationFrame(new Rectangle(240, 0, 180, 193), new Vector2(90, 96.5f), new Vector2(pacSize.X / 180f, pacSize.Y / 193f)));
            frameList.Add(new AnimationFrame(new Rectangle(465, 0, 195, 193), new Vector2(97.5f, 96.5f), new Vector2(pacSize.X / 195f, pacSize.Y / 193f)));
            pacman = new Pacman(pacmansprite, Color.White, new Vector2(TileSize.X * .5f + TileSize.X * 3, TileSize.Y * .5f + TileSize.Y * 5), scale: Vector2.One, frameList, TimeSpan.FromMilliseconds(100), 2f, ScreenManager, InputManager, PositionToTile);

            Texture2D ghostSprite = ContentManager.Load<Texture2D>("ghosts");

            Vector2 ghostSize = new Vector2(TileSize.X - 2, TileSize.Y - 2);

            frameList = new List<AnimationFrame>();
            frameList.Add(new AnimationFrame(new Rectangle(234, 47, 162, 148), new Vector2(81, 74), new Vector2(ghostSize.X / 162, ghostSize.Y / 148)));
            frameList.Add(new AnimationFrame(new Rectangle(46, 236, 158, 147), new Vector2(76, 73.5f), new Vector2(ghostSize.X / 158, ghostSize.Y / 147)));
            frameList.Add(new AnimationFrame(new Rectangle(45, 43, 161, 154), new Vector2(80.5f, 77), new Vector2(ghostSize.X / 161, ghostSize.Y / 154)));
            frameList.Add(new AnimationFrame(new Rectangle(235, 233, 160, 156), new Vector2(80, 78), new Vector2(ghostSize.X / 160, ghostSize.Y / 156)));
            ghosts.Add(new Ghost(ghostSprite, Color.White, new Vector2(TileSize.X * 1.5f, TileSize.Y * 1.5f), Vector2.One, frameList, 1f, PositionToTile));
            Objects.Add(pacman);
            foreach (Ghost ghost in ghosts)
            {
                Objects.Add(ghost);
            }
        }

        public override void Update(GameTime gameTime)
        {
            var ghostPos = PositionToTile(ghosts[0].Pos);
            var pacmanPos = PositionToTile(pacman.Pos);

            pacman.Update(gameTime);

            if (IsOnTile(ghosts[0].Pos, ghosts[0].HitBox))
            {
                ghosts[0].path = Traversals.AStar(PositionToTile(ghosts[0].Pos), PositionToTile(pacman.Pos), Heuristic, grid, ghosts[0].PreviousTile);
            }

            if (ghostPos.PositionInGrid == pacmanPos.PositionInGrid)
            {

            }

            base.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(pixelMap, new Rectangle(10, 10, pixelMap.Width, pixelMap.Height), Color.White);

            base.Draw(spriteBatch);
        }


        private int CalculateIndex(int x, int y, int width)
        {
            return width * y + x;
        }

        public List<Tile> GetNeighbors(int x, int y, Color[] pixels)
        {
            List<Tile> neighbors = new List<Tile>();
            Point tempPoint = new Point();
            int index = 0;


            //Calculates left neighbor
            tempPoint = new Point(y, x - 1);
            if (!PointToTile.ContainsKey(tempPoint))
            {
                index = CalculateIndex(tempPoint.X, tempPoint.Y, pixelMap.Width);
                if (index < pixels.Length && index >= 0)
                {
                    PointToTile.Add(tempPoint, pixels[index].CreateTile(GraphicsDeviceManager.GraphicsDevice, ScreenManager.Settings.ColorDictionary[pixels[index]], TileSize, tempPoint));
                    neighbors.Add(PointToTile[tempPoint]);
                }
            }
            else
            {
                neighbors.Add(PointToTile[tempPoint]);
            }


            //Calculates right neighbor
            tempPoint = new Point(y, x + 1);
            if (!PointToTile.ContainsKey(tempPoint))
            {
                index = CalculateIndex(tempPoint.X, tempPoint.Y, pixelMap.Width);
                if (index < pixels.Length && index >= 0)
                {
                    PointToTile.Add(tempPoint, pixels[index].CreateTile(GraphicsDeviceManager.GraphicsDevice, ScreenManager.Settings.ColorDictionary[pixels[index]], TileSize, tempPoint));
                    neighbors.Add(PointToTile[tempPoint]);
                }
            }
            else
            {
                neighbors.Add(PointToTile[tempPoint]);
            }


            //Calculates up neighbor
            tempPoint = new Point(y - 1, x);
            if (!PointToTile.ContainsKey(tempPoint))
            {
                index = CalculateIndex(tempPoint.X, tempPoint.Y, pixelMap.Width);
                if (index < pixels.Length && index >= 0)
                {
                    PointToTile.Add(tempPoint, pixels[index].CreateTile(GraphicsDeviceManager.GraphicsDevice, ScreenManager.Settings.ColorDictionary[pixels[index]], TileSize, tempPoint));
                    neighbors.Add(PointToTile[tempPoint]);
                }
            }
            else
            {
                neighbors.Add(PointToTile[tempPoint]);
            }



            //Calculates down neighbor
            tempPoint = new Point(y + 1, x);
            if (!PointToTile.ContainsKey(tempPoint))
            {
                index = CalculateIndex(tempPoint.X, tempPoint.Y, pixelMap.Width);
                if (index < pixels.Length && index >= 0)
                {
                    PointToTile.Add(tempPoint, pixels[index].CreateTile(GraphicsDeviceManager.GraphicsDevice, ScreenManager.Settings.ColorDictionary[pixels[index]], TileSize, tempPoint));
                    neighbors.Add(PointToTile[tempPoint]);
                }
            }
            else
            {
                neighbors.Add(PointToTile[tempPoint]);
            }


            return neighbors;
        }


        public Tile PositionToTile(Vector2 position)
        {
            return grid[(int)((position.X) / TileSize.X), (int)((position.Y) / TileSize.Y)];
        }


        private int Heuristic(Tile currentTile, Tile targetTile)
        {
            return (int)(Math.Abs(currentTile.PositionInGrid.X - targetTile.PositionInGrid.X) + Math.Abs(currentTile.PositionInGrid.Y - targetTile.PositionInGrid.Y));
        }

        private bool IsOnTile(Vector2 middlePos, Rectangle Hitbox)
        {
            Vector2 size = new Vector2(Hitbox.Width, Hitbox.Height);
            return PositionToTile(middlePos + size * 1 / 2).PositionInGrid == PositionToTile(middlePos - size * 1 / 2).PositionInGrid;
        }
    }
}
