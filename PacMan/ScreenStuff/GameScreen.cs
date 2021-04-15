
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PacMan.TraversalStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static PacMan.Enum;

namespace PacMan
{
    public class GameScreen : Screen
    {
        private Texture2D pixelMap;
        private Dictionary<Color, Func<Vector2, Vector2, Point, Tile>> textureDictionary;
        private Pacman pacman;
        private List<Ghost> ghosts;
        private List<Tile> walls;
        
        public Vector2 TileSize;
        private Dictionary<Point, Tile> pointToTile;
        private Tile[,] grid;
        private Rectangle screen => GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds;

        public GameScreen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            base.Load(graphics, content, bounds, screenManager, inputManager);

            textureDictionary = new Dictionary<Color, Func<Vector2, Vector2, Point, Tile>>
            {
                [Color.Black] = new Func<Vector2, Vector2, Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(CreatePixel(Color.White), Color.Black, scale, TileType.Wall, posInGrid)),
                [new Color(255, 28, 36)] = new Func<Vector2, Vector2, Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(CreatePixel(Color.White), Color.Red, scale, TileType.Background, posInGrid)),
                [new Color(237, 28, 36)] = new Func<Vector2, Vector2,   Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(CreatePixel(Color.White), Color.Red, scale, TileType.Background, posInGrid)),
                [new Color(34, 177, 76)] = new Func<Vector2, Vector2,   Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(CreatePixel(Color.White), Color.Green, scale, TileType.Background, posInGrid)),
                [new Color(255, 255, 255)] = new Func<Vector2, Vector2, Point, Tile>((Vector2 pos, Vector2 scale, Point posInGrid) => new Tile(CreatePixel(Color.White), Color.White, scale, TileType.Background, posInGrid)),
            };

            Heap<int> heap = new Heap<int>();
            heap.Add(65);
            heap.Add(75);
            heap.Add(34);
            heap.Add(1);
            heap.Add(5);

            List<int> orderedList = new List<int>();
            while(heap.Count > 0)
            {
                orderedList.Add(heap.Pop());
            }
            ;


            Init();
        }

        public void Init()
        {
            walls = new List<Tile>();
            ghosts = new List<Ghost>();
            pointToTile = new Dictionary<Point, Tile>();
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

                    if (!pointToTile.ContainsKey(tempPoint))
                    {
                        pointToTile.Add(tempPoint, textureDictionary[pixelColor](new Vector2(tempPoint.X, tempPoint.Y) * TileSize, TileSize, tempPoint));
                    }
                    Tile temp = pointToTile[tempPoint];

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

            var frameList = new List<AnimationFrame>();
            frameList.Add(new AnimationFrame(new Rectangle(0, 0, 136, 193), new Vector2(68, 96.5f)));
            frameList.Add(new AnimationFrame(new Rectangle(240, 0, 180, 193), new Vector2(90, 96.5f)));
            frameList.Add(new AnimationFrame(new Rectangle(465, 0, 195, 193), new Vector2(97.5f, 96.5f)));
            pacman = new Pacman(pacmansprite, Color.White, new Vector2(TileSize.X * .5f + TileSize.X * 3, TileSize.Y * .5f + TileSize.Y * 5), new Vector2(TileSize.X / frameList[1].HitBox.Width, TileSize.Y / frameList[1].HitBox.Height), frameList, TimeSpan.FromMilliseconds(100), 1.5f, 5, ScreenManager, InputManager);

            Texture2D ghostSprite = ContentManager.Load<Texture2D>("ghosts");

            frameList = new List<AnimationFrame>();
            frameList.Add(new AnimationFrame(new Rectangle(234, 47, 162, 148), new Vector2(81, 74)));
            frameList.Add(new AnimationFrame(new Rectangle(46, 236, 158, 147), new Vector2(76, 73.5f)));
            frameList.Add(new AnimationFrame(new Rectangle(45, 43, 161, 154), new Vector2(80.5f, 77)));
            frameList.Add(new AnimationFrame(new Rectangle(235, 233, 160, 156), new Vector2(80, 78)));
            ghosts.Add(new Ghost(ghostSprite, Color.White, new Vector2(TileSize.X * 1.5f, TileSize.Y * 1.5f), new Vector2(TileSize.X / frameList[0].HitBox.Width, TileSize.Y / frameList[0].HitBox.Height), frameList, 1f, 1, ScreenManager, InputManager, TimeSpan.FromMilliseconds(600)));
            Objects.Add(pacman);
            foreach(Ghost ghost in ghosts)
            {
                Objects.Add(ghost);
            }
        }

        public override void Update(GameTime gameTime)
        {
            var ghostPos = PositionToTile(ghosts[0].Pos);
            var pacmanPos = PositionToTile(pacman.Pos);

            
            ghosts[0].path = Traversals.AStar(PositionToTile(ghosts[0].Pos), PositionToTile(pacman.Pos), Heuristic, grid);
 
            if (ghostPos.PositionInGrid == pacmanPos.PositionInGrid)
            {

            }

            base.Update(gameTime);

            foreach (Tile wall in walls)
            {
                int index = 1;
                if (pacman.HitBox.Intersects(wall.HitBox))
                {
                    while (pacman.HitBox.Intersects(wall.HitBox))
                    {
                        pacman.Pos = pacman.PreviousPositions[index];
                        index++;
                    }
                    pacman.CurrentDirection = Directions.None;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(pixelMap, new Rectangle(10, 10, pixelMap.Width, pixelMap.Height), Color.White);

            base.Draw(spriteBatch);
        }


        public Texture2D CreatePixel(Color tint)
        {
            Texture2D returnTex = new Texture2D(GraphicsDeviceManager.GraphicsDevice, 1, 1);
            returnTex.SetData(new Color[] { tint });
            return returnTex;
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
            if (!pointToTile.ContainsKey(tempPoint))
            {
                index = CalculateIndex(tempPoint.X, tempPoint.Y, pixelMap.Width);
                if (index < pixels.Length && index >= 0)
                {
                    pointToTile.Add(tempPoint, textureDictionary[pixels[index]](new Vector2(tempPoint.X, tempPoint.Y) * TileSize, TileSize, tempPoint));
                    neighbors.Add(pointToTile[tempPoint]);
                }
            }
            else
            {
                neighbors.Add(pointToTile[tempPoint]);
            }


            //Calculates right neighbor
            tempPoint = new Point(y, x + 1);
            if (!pointToTile.ContainsKey(tempPoint))
            {
                index = CalculateIndex(tempPoint.X, tempPoint.Y, pixelMap.Width);
                if (index < pixels.Length && index >= 0)
                {
                    pointToTile.Add(tempPoint, textureDictionary[pixels[index]](new Vector2(tempPoint.X, tempPoint.Y) * TileSize, TileSize, tempPoint));
                    neighbors.Add(pointToTile[tempPoint]);
                }
            }
            else
            {
                neighbors.Add(pointToTile[tempPoint]);
            }


            //Calculates up neighbor
            tempPoint = new Point(y - 1, x);
            if (!pointToTile.ContainsKey(tempPoint))
            {
                index = CalculateIndex(tempPoint.X, tempPoint.Y, pixelMap.Width);
                if (index < pixels.Length && index >= 0)
                {
                    pointToTile.Add(tempPoint, textureDictionary[pixels[index]](new Vector2(tempPoint.X, tempPoint.Y) * TileSize, TileSize, tempPoint));
                    neighbors.Add(pointToTile[tempPoint]);
                }
            }
            else
            {
                neighbors.Add(pointToTile[tempPoint]);
            }



            //Calculates down neighbor
            tempPoint = new Point(y + 1, x);
            if (!pointToTile.ContainsKey(tempPoint))
            {
                index = CalculateIndex(tempPoint.X, tempPoint.Y, pixelMap.Width);
                if (index < pixels.Length && index >= 0)
                {
                    pointToTile.Add(tempPoint, textureDictionary[pixels[index]](new Vector2(tempPoint.X, tempPoint.Y) * TileSize, TileSize, tempPoint));
                    neighbors.Add(pointToTile[tempPoint]);
                }
            }
            else
            {
                neighbors.Add(pointToTile[tempPoint]);
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
    }
}
