using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class ColorWheel
    {
        private List<Sprite> sprites;
        public ColorWheel(Vector2 position, int size, GraphicsDeviceManager graphics)
        {
            Texture2D pixel = Color.White.CreatePixel(graphics.GraphicsDevice);
            sprites = new List<Sprite>();



            int y = (int)position.Y - size / 2;
            int x = (int)position.X - size / 2;
            int radiusSquared = size / 2 * size / 2;
            for (int z = 0; z < size; z ++)
            {
                for (int i = 0; i < size; i++)
                {
                    var deltaX = (i + x - position.X);
                    var deltaY = (y + z - position.Y);

                    if (deltaX * deltaX + deltaY * deltaY <= radiusSquared)
                    {
                        sprites.Add(new Sprite(pixel, Color.Black, new Vector2(i + x, y + z), Vector2.One, Vector2.Zero));
                    }


                }
            }
        }

        //Make a function that converts from cartesian to polar
        //The polar should return a (radius, theta)

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Sprite sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }

        public (float radius, float theta) CartesianToPolar(Point startPoint, Point endPoint)
        {
            (float radius, float theta) returnInfo;
            returnInfo.radius = Distance(startPoint, endPoint);
            returnInfo.theta = (float)Math.Atan((endPoint.Y - startPoint.Y)/(endPoint.X - startPoint.X));
            return returnInfo;
        }


        private float Distance(Point startPoint, Point endPoint)
        {
            float deltaX = endPoint.X - startPoint.X;
            float deltaY = endPoint.Y - startPoint.Y;

            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
        
    }
}
