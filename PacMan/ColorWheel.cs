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
        private Dictionary<Point, Color> posToColor;
        public ColorWheel(Vector2 position, int size, GraphicsDeviceManager graphics)
        {
            Texture2D pixel = Color.White.CreatePixel(graphics.GraphicsDevice);
            sprites = new List<Sprite>();

            posToColor = new Dictionary<Point, Color>();
            //Keep track of a dictionary that is from (x, y) -> Color


            int y = (int)position.Y - size / 2;
            int x = (int)position.X - size / 2;
            int radius = size / 2;
            for (int z = 0; z < size; z++)
            {
                for (int i = 0; i < size; i++)
                {
                    var deltaX = (i + x - position.X);
                    var deltaY = (y + z - position.Y);

                    var PolarInfo = CartesianToPolar(new Vector2(deltaX, deltaY));

                    if (PolarInfo.radius > radius)
                    {
                        continue;
                    }

                    //Map hsv values to our known values
                    //H: 0-360 <-> theta
                    //S: 0-1   <-> currentRadius / radius
                    //V: 0-1 (this is our alpha)  <-> 1 

                    //Make an hsv to rgb conversion

                    var convertedTheta = PolarInfo.theta + Math.PI;
                    convertedTheta = convertedTheta * 180 / Math.PI - 90;

                    Color currentColor = HsvToRgb(convertedTheta, PolarInfo.radius / radius, 1);
                    Vector2 tempPos = new Vector2(i + x, y + z);
                    posToColor.Add(tempPos.ToPoint(), currentColor);
                    sprites.Add(new Sprite(pixel, currentColor, tempPos, Vector2.One, Vector2.Zero));
                }
            }

            ;
        }

        //Make a function that converts from cartesian to polar
        //The polar should return a (radius, theta)

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }

        public (float radius, float theta) CartesianToPolar(Vector2 endPoint)
        {
            (float radius, float theta) returnInfo;
            returnInfo.radius = Vector2.Distance(Vector2.Zero, endPoint);
            returnInfo.theta = (float)Math.Atan2(endPoint.Y, endPoint.X);
            return returnInfo;
        }

        //Make a function that takes in a coordinate that is global relative to the screen
        //and convert it to a local coordinate relative to the color wheel(center of the circle is 0,0)
        //Step 2: return map[x, y]

        public Color HsvToRgb(double h, double S, double V)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            var r = Clamp((int)(R * 255.0));
            var g = Clamp((int)(G * 255.0));
            var b = Clamp((int)(B * 255.0));

            return new Color(r, g, b);
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }


        public Color? GetColor(Point targetPosition)
        {
            if (posToColor.ContainsKey(targetPosition))
            {
                return posToColor[targetPosition];
            }
            return null;
        }
    }
}
