﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PacMan.TraversalStuff;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using static PacMan.GhostManager;

namespace PacMan
{
    /// <summary>
    /// change all the lists to be one big span in ghostManager, // maybe not so necessary would make it much harder to read and it's actually only 2 lists
    /// 
    /// </summary>
     
    public class GameScreen : Screen
    {
        Texture2D pixelMap;
        Pacman pacman;
        GhostManager ghostManager;
        List<Ghost> ghosts;
        List<Tile> walls;
        List<Food> foods;
        Fruit fruit;
        int remainingFood;
        float ghostSpeed;

        public float percentEaten => (float)((foods.Count - remainingFood) / (float)foods.Count) * 100;
        public static Vector2 TileSize;
        public static Dictionary<Point, Tile> PointToTile;
        private Tile[,] grid;
        private Rectangle screen => GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds;

        Stopwatch watch;

        public static Sprite screenTint;

        public GameScreen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            base.Load(graphics, content, bounds, screenManager, inputManager);

            watch = new Stopwatch();
        }

        public override void Init()
        {
            screenTint = new Sprite(Game1.WhitePixel, Color.Red * .5f, Vector2.Zero, new Vector2(Bounds.Width, Bounds.Height), Vector2.Zero);
            screenTint.IsVisable = false;

            walls = new List<Tile>();
            ghosts = new List<Ghost>();
            foods = new List<Food>();
            PointToTile = new Dictionary<Point, Tile>();
            pixelMap = ContentManager.Load<Texture2D>("pacmanmap");
            grid = new Tile[pixelMap.Width, pixelMap.Height];

            Color[] pixels = new Color[pixelMap.Width * pixelMap.Height];
            pixelMap.GetData(pixels);

            float xChunk = screen.Width / pixelMap.Width;
            float yChunk = screen.Height / pixelMap.Height;

            TileSize = new Vector2(xChunk, yChunk);

            for (int y = 0; y < pixelMap.Height; y++)
            {
                for (int x = 0; x < pixelMap.Width; x++)
                {

                    int index = CalculateIndex(x, y, pixelMap.Width);
                    Color pixelColor = pixels[index];
                    Point tempPoint = new Point(x, y);

                    if (!PointToTile.ContainsKey(tempPoint))
                    {
                        PointToTile.Add(tempPoint, pixelColor.CreateTile(GraphicsDeviceManager.GraphicsDevice, ScreenManager.Settings.ColorDictionary[pixelColor], TileSize, tempPoint));
                    }
                    Tile temp = PointToTile[tempPoint];

                    grid[y, x] = temp;

                    List<Tile> neighbors = new List<Tile>();

                    temp.Neighbors = GetNeighbors(x, y, pixels);
                    Objects.Add(temp);

                    var tile = Objects[y * pixelMap.Width + x] as Tile;

                    if (tile.TileType == TileTypes.Wall)
                    {
                        walls.Add(tile);
                    }
                }
            }
            grid[6, 9].Neighbors.Remove(grid[7,9]);

            foreach(Tile tile in grid)
            {
                if (tile.TileType == TileTypes.Background)
                {
                    Food current = new Food(tile, GraphicsDeviceManager.GraphicsDevice);
                    Objects.Add(current);
                    foods.Add(current);
                }
            }
            //foods.Add(new Food(PositionToTile(pacman.Pos, grid), GraphicsDeviceManager.GraphicsDevice));
            remainingFood = foods.Count;

            Texture2D pacmansprite = ContentManager.Load<Texture2D>("pacmansprite");

            //What is a good width and height for the pacman?
            #region makePacman
            float pacSpeed = TileSize.X / 10;
            Vector2 pacSize = new Vector2(TileSize.X - pacSpeed, TileSize.Y - pacSpeed);

            var frameList = new List<AnimationFrame>();
            frameList.Add(new AnimationFrame(new Rectangle(0, 0, 136, 193), new Vector2(68, 96.5f), new Vector2(pacSize.X / 136f, pacSize.Y / 193f)));
            frameList.Add(new AnimationFrame(new Rectangle(240, 0, 180, 193), new Vector2(90, 96.5f), new Vector2(pacSize.X / 180f, pacSize.Y / 193f)));
            frameList.Add(new AnimationFrame(new Rectangle(465, 0, 195, 193), new Vector2(97.5f, 96.5f), new Vector2(pacSize.X / 195f, pacSize.Y / 193f)));
            pacman = new Pacman(pacmansprite, Color.White, new Vector2(TileSize.X * .5f + TileSize.X * 3, TileSize.Y * .5f + TileSize.Y * 5), scale: Vector2.One, frameList, TimeSpan.FromMilliseconds(50), pacSpeed, ScreenManager, InputManager, grid);

            #endregion

            #region makeGhosts
            Texture2D ghostSprite = ContentManager.Load<Texture2D>("ghosts");

            ghostSpeed = TileSize.X / 30;
            Vector2 ghostSize = new Vector2(TileSize.X - ghostSpeed * 2, TileSize.Y - ghostSpeed * 2);

            frameList = new List<AnimationFrame>();
            frameList.Add(new Rectangle(235, 45, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(45, 235, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(45, 45, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(235, 235, 160, 160).CreateFrame(true, ghostSize));
            ghosts.Add(new Ghost(ghostSprite, Color.White, new Vector2(TileSize.X * 1.5f, TileSize.Y * 1.5f), Vector2.One, frameList, ghostSpeed, grid, ScreenManager));

            frameList = new List<AnimationFrame>();
            frameList.Add(new Rectangle(235, 420, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(45, 610, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(45, 420, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(235, 610, 160, 160).CreateFrame(true, ghostSize));
            ghosts.Add(new Ghost(ghostSprite, Color.White, new Vector2(TileSize.X * 1.5f, TileSize.Y * 1.5f), Vector2.One, frameList, ghostSpeed, grid, ScreenManager));

            frameList = new List<AnimationFrame>();
            frameList.Add(new Rectangle(635, 40, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(445, 230, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(445, 40, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(635, 230, 160, 160).CreateFrame(true, ghostSize));
            ghosts.Add(new Ghost(ghostSprite, Color.White, new Vector2(TileSize.X * 1.5f, TileSize.Y * 1.5f), Vector2.One, frameList, ghostSpeed, grid, ScreenManager));

            frameList = new List<AnimationFrame>();
            frameList.Add(new Rectangle(635, 420, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(445, 610, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(445, 420, 160, 160).CreateFrame(true, ghostSize));
            frameList.Add(new Rectangle(635, 610, 160, 160).CreateFrame(true, ghostSize));
            ghosts.Add(new Ghost(ghostSprite, Color.White, new Vector2(TileSize.X * 1.5f, TileSize.Y * 1.5f), Vector2.One, frameList, ghostSpeed, grid, ScreenManager));
            #endregion

            #region makeFruit
            Vector2 fruitPos = CalculateNewFruitPos();

            Texture2D fruits = ContentManager.Load<Texture2D>("pacmanfruit");
            List<AnimationFrame> fruitFrames = new List<AnimationFrame>();
            fruitFrames.Add(new Rectangle(0, 0, 52, 52).CreateFrame(true, TileSize));
            fruitFrames.Add(new Rectangle(0, 65, 49, 52).CreateFrame(true, TileSize));
            fruitFrames.Add(new Rectangle(1, 130, 52, 52).CreateFrame(true, TileSize));
            fruitFrames.Add(new Rectangle(1, 195, 52, 56).CreateFrame(true, TileSize));
            fruitFrames.Add(new Rectangle(1, 260, 53, 52).CreateFrame(true, TileSize));
            fruitFrames.Add(new Rectangle(6, 321, 48, 61).CreateFrame(true, TileSize));
            fruitFrames.Add(new Rectangle(2, 394, 48, 49).CreateFrame(true, TileSize));
            fruitFrames.Add(new Rectangle(10, 455, 32, 57).CreateFrame(true, TileSize));

            #endregion

            #region makeGhostManager
            Texture2D specialGhosts = ContentManager.Load<Texture2D>("SpecialGhostSpriteSheet");
            List<AnimationFrame> specialGhostFrames = new List<AnimationFrame>();
            specialGhostFrames.Add(new Rectangle(0, 0, 58, 58).CreateFrame(true, ghostSize));
            specialGhostFrames.Add(new Rectangle(0, 0, 58, 58).CreateFrame(true, ghostSize));
            specialGhostFrames.Add(new Rectangle(0, 64, 58, 58).CreateFrame(true, ghostSize));
            specialGhostFrames.Add(new Rectangle(0, 64, 58, 58).CreateFrame(true, ghostSize));

            specialGhostFrames.Add(new Rectangle(0, 128, 58, 58).CreateFrame(true, ghostSize));
            specialGhostFrames.Add(new Rectangle(0, 128, 58, 58).CreateFrame(true, ghostSize));
            specialGhostFrames.Add(new Rectangle(0, 192, 58, 58).CreateFrame(true, ghostSize));
            specialGhostFrames.Add(new Rectangle(0, 192, 58, 58).CreateFrame(true, ghostSize));

            specialGhostFrames.Add(new Rectangle(78, 0, 58, 58).CreateFrame(true,   ghostSize));
            specialGhostFrames.Add(new Rectangle(78, 64, 58, 58).CreateFrame(true,  ghostSize));
            specialGhostFrames.Add(new Rectangle(78, 128, 58, 58).CreateFrame(true, ghostSize));
            specialGhostFrames.Add(new Rectangle(78, 192, 58, 58).CreateFrame(true, ghostSize));

            ghostManager = new GhostManager(ghosts, specialGhosts, specialGhostFrames, grid, ref pacman);
            #endregion

            fruit = new Fruit(fruits, Color.Transparent, Color.White, fruitPos, Vector2.Zero, fruitFrames);
            Objects.Add(pacman);
            Objects.Add(fruit);

            Objects.Add(screenTint);
            watch.Start();
        }

        public override void Update(GameTime gameTime)
        {

            if (watch.ElapsedMilliseconds < 2000) return;

            ghostManager.Update(gameTime);

            UpdateFood();

            base.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(pixelMap, new Rectangle(10, 10, pixelMap.Width, pixelMap.Height), Color.White);

            base.Draw(spriteBatch);
            ghostManager.Draw(spriteBatch);
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
            tempPoint = new Point(x - 1, y);
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
            tempPoint = new Point(x + 1, y);
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
            tempPoint = new Point(x, y - 1);
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
            tempPoint = new Point(x, y + 1);
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


        public static Tile PositionToTile(Vector2 position, Tile[,] grid)
        {
            int xPoint = (int)(position.X / TileSize.X);
            int yPoint = (int)(position.Y / TileSize.Y);
            if(xPoint < 0)
            {
                xPoint = 0;
            }
            else if(xPoint >= 19)
            {
                xPoint = 18;
            }

            if(yPoint < 0)
            {
                yPoint = 0;
            }
            else if(yPoint >= 19)
            {
                yPoint = 18;
            }
            return grid[yPoint, xPoint];
        }

        public static Point PositionToGridPoint(Vector2 position)
        {
            int xPoint = (int)(position.X / TileSize.X);
            int yPoint = (int)(position.Y / TileSize.Y);
            if (xPoint < 0)
            {
                xPoint = 0;
            }
            else if (xPoint >= 19)
            {
                xPoint = 18;
            }

            if (yPoint < 0)
            {
                yPoint = 0;
            }
            else if (yPoint >= 19)
            {
                yPoint = 18;
            }
            return new Point(xPoint, yPoint);
        }

        public static int Heuristic(Tile currentTile, Tile targetTile)
        {
            return (int)(Math.Abs(currentTile.PositionInGrid.X - targetTile.PositionInGrid.X) + Math.Abs(currentTile.PositionInGrid.Y - targetTile.PositionInGrid.Y));
        }

        private bool IsOnTile(Vector2 middlePos, Rectangle Hitbox)
        {
            Vector2 size = new Vector2(Hitbox.Width, Hitbox.Height);
            return PositionToTile(middlePos + size * 1 / 2, grid).PositionInGrid == PositionToTile(middlePos - size * 1 / 2, grid).PositionInGrid;
        }

        private Vector2 CalculateNewFruitPos()
        {
            Vector2 fruitPos;
            Random random = new Random();
            do
            {
                fruitPos = new Vector2(random.Next(0, pixelMap.Width), random.Next(0, pixelMap.Height));
            } while (PointToTile[fruitPos.ToPoint()].TileType != TileTypes.Background);
            fruitPos = fruitPos * TileSize;
            fruitPos += TileSize / 2;

            return fruitPos;
        }

        private void UpdateFood()
        {
            if (fruit.CurrentState != FruitStates.ScaleIn && fruit.CurrentState != FruitStates.ScaleOut && pacman.HitBox.Intersects(fruit.HitBox))
            {
                fruit.ChangeFruit(CalculateNewFruitPos());
                ///If I add in the fade out state I should check for it here as well
                if (ghostManager.GeneralState != GhostStates.RunAway)
                {
                    ghostManager.SwitchMode(GhostStates.RunAway);
                }
                ghostManager.StopWatch.Restart();
            }

            foreach (Food food in foods)
            {
                if (pacman.HitBox.Intersects(food.HitBox) && food.IsVisable == true)
                {
                    food.IsVisable = false;
                    remainingFood--;
                }
            }

            if (percentEaten > 50 && ghosts[0].speed == ghostSpeed)
            {
                foreach (Ghost ghost in ghosts)
                {
                    ghost.speed = ghost.speed * 1.5f;
                }
            }

            if (percentEaten == 100)
            {
                while (true) ;
            }

            if(screenTint.IsVisable == true)
            {
                ResetGame();
            }
        }

        private void ResetGame()
        {
            fruit.ChangeFruit(CalculateNewFruitPos());
            fruit.CurrentIndex = 0;
            foreach (Food food in foods)
            {
                food.IsVisable = true;
                remainingFood = foods.Count;
            }
            for(int i = 0; i < ghosts.Count; i ++)
            {
                ghosts[i].speed = ghostSpeed;
            }
            ghostManager.Reset();
            var temp = ScreenManager.Settings.TeleportDictionary;
            foreach(var kvp in temp)
            {
                kvp.Key.Neighbors.Remove(kvp.Value);
            }
            ScreenManager.Settings.TeleportDictionary = new Dictionary<Tile, Tile>();
        }
    }
}