using Microsoft.Xna.Framework;
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
    public class GameScreen : Screen
    {
        Texture2D pixelMap;
        Pacman pacman;
        GhostManager ghostManager;
        List<Ghost> ghosts;
        List<Tile> walls;
        Fruit fruit;

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
            PointToTile = new Dictionary<Point, Tile>();
            pixelMap = ContentManager.Load<Texture2D>("pacmanmap");
            grid = new Tile[pixelMap.Width, pixelMap.Height];

            Color[] pixels = new Color[pixelMap.Width * pixelMap.Height];
            pixelMap.GetData(pixels);

            float xChunk = screen.Width / pixelMap.Width;
            float yChunk = screen.Height / pixelMap.Height;

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

                    if (tile.TileType == TileTypes.Wall)
                    {
                        walls.Add(tile);
                    }
                }
            }

            Texture2D pacmansprite = ContentManager.Load<Texture2D>("pacmansprite");

            //What is a good width and height for the pacman?

            float pacSpeed = TileSize.X / 10;
            Vector2 pacSize = new Vector2(TileSize.X - pacSpeed, TileSize.Y - pacSpeed);

            var frameList = new List<AnimationFrame>();
            frameList.Add(new AnimationFrame(new Rectangle(0, 0, 136, 193), new Vector2(68, 96.5f), new Vector2(pacSize.X / 136f, pacSize.Y / 193f)));
            frameList.Add(new AnimationFrame(new Rectangle(240, 0, 180, 193), new Vector2(90, 96.5f), new Vector2(pacSize.X / 180f, pacSize.Y / 193f)));
            frameList.Add(new AnimationFrame(new Rectangle(465, 0, 195, 193), new Vector2(97.5f, 96.5f), new Vector2(pacSize.X / 195f, pacSize.Y / 193f)));
            pacman = new Pacman(pacmansprite, Color.White, new Vector2(TileSize.X * .5f + TileSize.X * 3, TileSize.Y * .5f + TileSize.Y * 5), scale: Vector2.One, frameList, TimeSpan.FromMilliseconds(50), pacSpeed, ScreenManager, InputManager, PositionToTile);

            Texture2D ghostSprite = ContentManager.Load<Texture2D>("ghosts");

            float ghostSpeed = TileSize.X / 40;
            Vector2 ghostSize = new Vector2(TileSize.X - ghostSpeed, TileSize.Y - ghostSpeed);

            frameList = new List<AnimationFrame>();
            frameList.Add(new AnimationFrame(new Rectangle(234, 47, 162, 148), new Vector2(81, 74), new Vector2(ghostSize.X / 162, ghostSize.Y / 148)));
            frameList.Add(new AnimationFrame(new Rectangle(46, 236, 158, 147), new Vector2(76, 73.5f), new Vector2(ghostSize.X / 158, ghostSize.Y / 147)));
            frameList.Add(new AnimationFrame(new Rectangle(45, 43, 161, 154), new Vector2(80.5f, 77), new Vector2(ghostSize.X / 161, ghostSize.Y / 154)));
            frameList.Add(new AnimationFrame(new Rectangle(235, 233, 160, 156), new Vector2(80, 78), new Vector2(ghostSize.X / 160, ghostSize.Y / 156)));
            ghosts.Add(new Ghost(ghostSprite, Color.White, new Vector2(TileSize.X * 1.5f, TileSize.Y * 1.5f), Vector2.One, frameList, ghostSpeed, PositionToTile));

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


            Texture2D specialGhosts = ContentManager.Load<Texture2D>("SpecialGhostSpriteSheet");
            List<AnimationFrame> specialGhostFrames = new List<AnimationFrame>();
            specialGhostFrames.Add(new Rectangle(0, 0, 58, 58).CreateFrame(true, TileSize));
            specialGhostFrames.Add(new Rectangle(0, 0, 58, 58).CreateFrame(true, TileSize));
            specialGhostFrames.Add(new Rectangle(0, 64, 58, 58).CreateFrame(true, TileSize));
            specialGhostFrames.Add(new Rectangle(0, 64, 58, 58).CreateFrame(true, TileSize));

            specialGhostFrames.Add(new Rectangle(0, 128, 58, 58).CreateFrame(true, TileSize));
            specialGhostFrames.Add(new Rectangle(0, 128, 58, 58).CreateFrame(true, TileSize));
            specialGhostFrames.Add(new Rectangle(0, 192, 58, 58).CreateFrame(true, TileSize));
            specialGhostFrames.Add(new Rectangle(0, 192, 58, 58).CreateFrame(true, TileSize));

            specialGhostFrames.Add(new Rectangle(78, 0, 58, 58).CreateFrame(true, TileSize));
            specialGhostFrames.Add(new Rectangle(78, 64, 58, 58).CreateFrame(true, TileSize));
            specialGhostFrames.Add(new Rectangle(78, 128, 58, 58).CreateFrame(true, TileSize));
            specialGhostFrames.Add(new Rectangle(78, 192, 58, 58).CreateFrame(true, TileSize));

            ghostManager = new GhostManager(ghosts, specialGhosts, specialGhostFrames, grid, ref pacman);

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
    }
}