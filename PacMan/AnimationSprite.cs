using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class AnimationSprite : Sprite
    {
        public List<AnimationFrame> Frames { get; internal set; }
        protected int currentIndex;
        public AnimationFrame CurrentFrame => Frames[currentIndex];
        public float Rotation;

        TimeSpan elapsedTime;
        TimeSpan timeBetweenFrames;

        private Texture2D substitutionTex;
        public AnimationSprite(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames) 
            : base(new Texture2D(tex.GraphicsDevice, frames[1].HitBox.Width, frames[1].HitBox.Height), tint, pos, scale)
        {
            Frames = frames;
            elapsedTime = TimeSpan.Zero;
            this.timeBetweenFrames = timeBetweenFrames;
            Rotation = 0;
            substitutionTex = tex;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if(elapsedTime >= timeBetweenFrames)
            {
                elapsedTime = TimeSpan.Zero;
                currentIndex++;
                currentIndex = currentIndex % Frames.Count;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(substitutionTex, Pos, CurrentFrame.HitBox, Tint, Rotation, CurrentFrame.Origin, Scale, SpriteEffects.None, 1);
        }

    }
}
