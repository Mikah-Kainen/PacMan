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
        public int currentIndex;
        public AnimationFrame CurrentFrame => Frames[currentIndex];
        public float Rotation;

        TimeSpan elapsedTime;
        TimeSpan timeBetweenFrames;

        public override Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)(Pos.X - CurrentFrame.HitBox.Width * Scale.X * CurrentFrame.Scale.X / 2), (int)(Pos.Y - CurrentFrame.HitBox.Height * Scale.Y * CurrentFrame.Scale.Y / 2), (int)(CurrentFrame.HitBox.Width * Scale.X * CurrentFrame.Scale.X), (int)(CurrentFrame.HitBox.Height * Scale.Y * CurrentFrame.Scale.Y));
            }
        }

        public AnimationSprite(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames) 
            : base(tex, tint, pos, scale, Vector2.Zero)
        {
            Frames = frames;
            elapsedTime = TimeSpan.Zero;
            this.timeBetweenFrames = timeBetweenFrames;
            Rotation = 0;
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
            spriteBatch.Draw(Tex, Pos, CurrentFrame.HitBox, Tint, Rotation, CurrentFrame.Origin, Scale * CurrentFrame.Scale, SpriteEffects.None, 1);
        }

    }
}
