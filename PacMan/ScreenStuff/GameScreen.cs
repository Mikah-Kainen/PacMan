
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
        private Dictionary<Color, Func<Vector2, Vector2, Tile>> textureDictionary;
        private Pacman pacman;
        private List<Tile> walls;
        public Vector2 TileSize;
        private Rectangle screen => GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds;

        public GameScreen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            base.Load(graphics, content, bounds, screenManager, inputManager);

            textureDictionary = new Dictionary<Color, Func<Vector2, Vector2, Tile>>
            {
                [Color.Black] = new Func<Vector2, Vector2, Tile>((Vector2 pos, Vector2 scale) => new Tile(CreatePixel(Color.Black), Color.White, pos, scale, TileType.Wall)),
                [new Color(255, 28, 36)] = new Func<Vector2, Vector2, Tile>((Vector2 pos, Vector2 scale) => new Tile(CreatePixel(Color.Red), Color.White, pos, scale, TileType.Background)),
                [new Color(237, 28, 36)] = new Func<Vector2, Vector2, Tile>((Vector2 pos, Vector2 scale) => new Tile(CreatePixel(Color.Red), Color.White, pos, scale, TileType.Background)),
                [new Color(34, 177, 76)] = new Func<Vector2, Vector2, Tile>((Vector2 pos, Vector2 scale) => new Tile(CreatePixel(Color.Green), Color.White, pos, scale, TileType.Background)),
                [new Color(255, 255, 255)] = new Func<Vector2, Vector2, Tile>((Vector2 pos, Vector2 scale) => new Tile(CreatePixel(Color.White), Color.White, pos, scale, TileType.Background)),
            };

            Init();
        }

        public void Init()
        {
            walls = new List<Tile>();
            pixelMap = ContentManager.Load<Texture2D>("pacmanmap");

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
                    Objects.Add(textureDictionary[pixelColor](new Vector2(x, y) * TileSize, TileSize));

                    var tile = Objects[y + x * pixelMap.Width] as Tile;

                    if (tile.TileType == TileType.Wall)
                    {
                        walls.Add(tile);
                    }
                    //You now have the pixel color, determine what texture this should map to from your pixel map
                }
            }

            Texture2D pacmansprite = ContentManager.Load<Texture2D>("pacmansprite");

            var frameList = new List<AnimationFrame>();
            frameList.Add(new AnimationFrame(new Rectangle(0, 0, 136, 193), new Vector2(68, 96.5f)));
            frameList.Add(new AnimationFrame(new Rectangle(240, 0, 180, 193), new Vector2(90, 96.5f)));
            frameList.Add(new AnimationFrame(new Rectangle(465, 0, 195, 193), new Vector2(97.5f, 96.5f)));
            pacman = new Pacman(pacmansprite, Color.White, new Vector2(screen.Width / 2f, screen.Height / 2f - 50), new Vector2(.3f, .3f), frameList, TimeSpan.FromMilliseconds(100), 1.5f, 5, ScreenManager, InputManager);
            Objects.Add(pacman);
        }

        public override void Update(GameTime gameTime)
        {

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

        private Vector2 GetPacTile()
        {
            return Vector2.Zero;
        }

        private Vector2 GetBlinkyTile()
        {
            return Vector2.Zero;
        }

        private Vector2 GetInkyTile()
        {
            return Vector2.Zero;
        }

        private Vector2 GetPinkyTile()
        {
            return Vector2.Zero;
        }
    }
}
