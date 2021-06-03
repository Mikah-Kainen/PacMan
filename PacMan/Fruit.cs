using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PacMan
{
    public class Fruit : Sprite
    {
        LerpData<Vector2> scaleLerp;
        LerpData<Color> tintLerp;
        Rectangle currentFrame;
        Vector2 endScale;
        FruitStates currentState = FruitStates.ScaleIn;
        Stopwatch stopwatch = new Stopwatch();
        LerpData<Vector2> pulseLerp;

        private Vector2 goalScaleUp;
        private Vector2 goalScaleDown;

        float scaleStep;
        public Fruit(Texture2D tex, Rectangle currentFrame, Color startTint, Color endTint, Vector2 pos, Vector2 startScale, Vector2 endScale, Vector2 origin)
           : base(tex, startTint, pos, startScale, new Vector2(currentFrame.Width * origin.X, currentFrame.Height * origin.Y))
        {
            scaleStep = endScale.X / 5000f;
            scaleLerp = new LerpData<Vector2>(startScale, new Vector2(endScale.X / currentFrame.Width, endScale.Y / currentFrame.Width), scaleStep, Vector2.Lerp);
            tintLerp = new LerpData<Color>(startTint, endTint, (float)endTint.A / 1500f, Color.Lerp);
            this.endScale = endScale;
            this.currentFrame = currentFrame;

            goalScaleDown = scaleLerp.End * 0.9f;
            goalScaleUp = scaleLerp.End;
        }


        public override void Update(GameTime gameTime)
        {
            switch(currentState)
            {
                case FruitStates.ScaleIn:
                    Scale = scaleLerp.GetCurrent();
                    if (scaleLerp.IsComplete)
                    {
                        stopwatch.Restart();
                        currentState = FruitStates.Delay;
                    }
                    break;

                case FruitStates.Delay:
                    if(stopwatch.ElapsedMilliseconds > 1000)
                    {
                        pulseLerp = new LerpData<Vector2>(goalScaleUp, goalScaleDown, scaleStep * 2, Vector2.Lerp);   
                        currentState = FruitStates.ScaleDown;
                    }
                    break;

                case FruitStates.ScaleUp:
                    Scale = pulseLerp.GetCurrent();
                    if(pulseLerp.IsComplete)
                    {
                        pulseLerp = new LerpData<Vector2>(goalScaleUp, goalScaleDown, scaleStep * 2, Vector2.Lerp);
                        currentState = FruitStates.ScaleDown;
                    }
                    break;

                case FruitStates.ScaleDown:
                    Scale = pulseLerp.GetCurrent();
                    if(pulseLerp.IsComplete)
                    {
                        pulseLerp = new LerpData<Vector2>(goalScaleDown, goalScaleUp, scaleStep * 2, Vector2.Lerp);
                        currentState = FruitStates.ScaleUp;
                    }
                    break;
            }
            Tint = tintLerp.GetCurrent();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Tex, Pos, currentFrame, Tint, 0, Origin, Scale, SpriteEffects.None, 1);
        }


        public void ChangeFruit(Rectangle newFruitFrame)
        {
            base.Origin = new Vector2(newFruitFrame.Width * Origin.X, newFruitFrame.Height * Origin.Y);
            scaleLerp.End = new Vector2(endScale.X / newFruitFrame.Width, endScale.Y / newFruitFrame.Height);
        }
    }
}
