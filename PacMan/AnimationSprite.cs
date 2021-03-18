using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class AnimationSprite : Sprite
    {
        public List<AnimationFrame> Frames { get; private set; }
        int currentIndex;
        public AnimationFrame CurrentFrame => Frames[currentIndex];

        TimeSpan elapsedTime;
        TimeSpan timeBetweenFrames;
        public AnimationSprite(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames) 
            : base(tex, tint, pos, scale)
        {
            Frames = frames;
            elapsedTime = TimeSpan.Zero;
            this.timeBetweenFrames = timeBetweenFrames;
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
            spriteBatch.Draw(Tex, Pos, CurrentFrame.HitBox, Tint, 0, CurrentFrame.Origin, Scale, SpriteEffects.None, 1);
        }

    }
}
