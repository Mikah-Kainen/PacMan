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

        /// 
        /// //// End loop make the fruit change when pacman eats it!!!!!!(add a check for that)
        /// Change the ghost to have a runaway mode then add in the other ghosts


        LerpData<Vector2> scaleLerp;
        LerpData<Color> tintLerp;
        FruitStates currentState = FruitStates.ScaleIn;
        Stopwatch stopwatch = new Stopwatch();
        LerpData<Vector2> pulseLerp;
        List<AnimationFrame> frames;
        int currentIndex;
        int count;
        AnimationFrame currentFrame => frames[currentIndex];
        public override Rectangle HitBox => currentFrame.HitBox;

        private Vector2 goalScaleUp => scaleLerp.End * .9f;
        private Vector2 goalScaleDown => scaleLerp.End;

        float scaleStep;
        public Fruit(Texture2D tex, Color startTint, Color endTint, Vector2 pos, Vector2 startScale, List<AnimationFrame> frames)
           : base(tex, startTint, pos, startScale, frames[0].Origin)
        {
            this.frames = frames;
            currentIndex = 0;
            scaleStep = frames[currentIndex].Scale.X / 100f;
            scaleLerp = new LerpData<Vector2>(startScale, frames[currentIndex].Scale, scaleStep, Vector2.Lerp);
            tintLerp = new LerpData<Color>(startTint, endTint, (float)endTint.A / 1500f, Color.Lerp);
            count = 0;
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
                    if(stopwatch.ElapsedMilliseconds > 500)
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
                        count++;
                        if(count > 0)
                        {
                            count = 0;
                            ChangeFruit(Pos);
                        }
                        pulseLerp = new LerpData<Vector2>(goalScaleDown, goalScaleUp, scaleStep * 2, Vector2.Lerp);
                        currentState = FruitStates.ScaleUp;
                    }
                    break;
            }
            Tint = tintLerp.GetCurrent();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Tex, Pos, HitBox, Tint, 0, Origin, Scale, SpriteEffects.None, 1);
        }


        public void ChangeFruit(Vector2 newPos)
        {
            currentIndex = (currentIndex + 1) % frames.Count;
            Origin = currentFrame.Origin;
            scaleLerp = new LerpData<Vector2>(scaleLerp.Start, frames[currentIndex].Scale, scaleStep, Vector2.Lerp);
            Pos = newPos;
        }
    }
}
