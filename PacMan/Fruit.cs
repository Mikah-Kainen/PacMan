using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Fruit : Sprite
    {
        LerpData<Vector2> scaleLerp;
        LerpData<Color> tintLerp;
        Rectangle currentFrame;
        Vector2 endScale;
        public Fruit(Texture2D tex, Rectangle currentFrame, Color startTint, Color endTint, Vector2 pos, Vector2 startScale, Vector2 endScale, Vector2 origin)
           : base(tex, startTint, pos, startScale, new Vector2(currentFrame.Width * origin.X, currentFrame.Height * origin.Y))
        {
            scaleLerp = new LerpData<Vector2>(startScale, new Vector2(endScale.X / currentFrame.Width, endScale.Y / currentFrame.Width), endScale.X / 5000f, Vector2.Lerp);
            tintLerp = new LerpData<Color>(startTint, endTint, (float)endTint.A / 1500f, Color.Lerp);
            this.endScale = endScale;
            this.currentFrame = currentFrame;
        }


        public override void Update(GameTime gameTime)
        {
            Scale = scaleLerp.GetCurrent();
            Tint = tintLerp.GetCurrent();
        }


        public void Draw(SpriteBatch spriteBatch)
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
