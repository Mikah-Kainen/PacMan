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
        /// Change the ghost to have a runaway mode then add in the other ghosts


        Vector2 startScale;
        LerpData<Vector2> scaleLerp;
        LerpData<Color> tintLerp;

        public FruitStates CurrentState = FruitStates.ScaleIn;
        Stopwatch stopwatch = new Stopwatch();

        List<AnimationFrame> frames;
        int currentIndex;
        AnimationFrame currentFrame => frames[currentIndex];
        public Rectangle SourceRectangle => currentFrame.HitBox;
        public override Rectangle HitBox => new Rectangle((int)(Pos.X - currentFrame.Origin.X * Scale.X *currentFrame.Scale.X), (int)(Pos.Y - currentFrame.Origin.Y * Scale.Y * currentFrame.Scale.Y), (int)(currentFrame.HitBox.Width * Scale.X * currentFrame.Scale.X), (int)(currentFrame.HitBox.Height * Scale.Y * currentFrame.Scale.Y));
        private Vector2 goalScaleUp => frames[currentIndex].Scale;
        private Vector2 goalScaleDown => frames[currentIndex].Scale * .9f;

        float scaleStep;

        private Vector2 nextFruitPos;
        public Fruit(Texture2D tex, Color startTint, Color endTint, Vector2 pos, Vector2 startScale, List<AnimationFrame> frames)
           : base(tex, startTint, pos, startScale, frames[0].Origin)
        {
            this.frames = frames;
            currentIndex = 0;
            this.startScale = startScale;
            scaleStep = frames[currentIndex].Scale.X / 100f;
            scaleLerp = new LerpData<Vector2>(startScale, frames[currentIndex].Scale, scaleStep, Vector2.Lerp);
            tintLerp = new LerpData<Color>(startTint, endTint, (float)endTint.A / 1500f, Color.Lerp);
        }


        public override void Update(GameTime gameTime)
        {
            switch(CurrentState)
            {
                case FruitStates.ScaleIn:
                    Scale = scaleLerp.GetCurrent();
                    if (scaleLerp.IsComplete)
                    {
                        stopwatch.Restart();
                        CurrentState = FruitStates.Delay;
                    }
                    break;

                case FruitStates.Delay:
                    if(stopwatch.ElapsedMilliseconds > 500)
                    {
                        scaleLerp = new LerpData<Vector2>(goalScaleUp, goalScaleDown, scaleStep * 2, Vector2.Lerp);   
                        CurrentState = FruitStates.ScaleDown;
                    }
                    break;

                case FruitStates.ScaleUp:
                    Scale = scaleLerp.GetCurrent();
                    if(scaleLerp.IsComplete)
                    {
                        scaleLerp = new LerpData<Vector2>(goalScaleUp, goalScaleDown, scaleStep * 2, Vector2.Lerp);
                        CurrentState = FruitStates.ScaleDown;
                    }
                    break;

                case FruitStates.ScaleDown:
                    Scale = scaleLerp.GetCurrent();
                    if(scaleLerp.IsComplete)
                    {
                        scaleLerp = new LerpData<Vector2>(goalScaleDown, goalScaleUp, scaleStep * 2, Vector2.Lerp);
                        CurrentState = FruitStates.ScaleUp;
                    }
                    break;

                case FruitStates.ScaleOut:
                    Scale = scaleLerp.GetCurrent();
                    if(scaleLerp.IsComplete)
                    {
                        FinishChangeFruit(nextFruitPos);
                    }
                    break;
            }
            Tint = tintLerp.GetCurrent();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Tex, Pos, SourceRectangle, Tint, 0, Origin, Scale, SpriteEffects.None, 1);
        }

        public void ChangeFruit(Vector2 newPos)
        {
            scaleLerp = new LerpData<Vector2>(frames[currentIndex].Scale, startScale, scaleStep * 15, Vector2.Lerp);
            CurrentState = FruitStates.ScaleOut;
            nextFruitPos = newPos;
        }

        private void FinishChangeFruit(Vector2 newPos)
        {
            currentIndex = (currentIndex + 1) % frames.Count;
            Origin = currentFrame.Origin;
            scaleLerp = new LerpData<Vector2>(startScale, frames[currentIndex].Scale, scaleStep, Vector2.Lerp);
            Pos = newPos;
            CurrentState = FruitStates.ScaleIn;
        }
    }
}
