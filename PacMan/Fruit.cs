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


        public Fruit(Texture2D tex, Color startTint, Color endTint, Vector2 pos, Vector2 startScale, Vector2 endScale, Vector2 origin)
           : base(tex, startTint, pos, startScale, origin)
        {
            scaleLerp = new LerpData<Vector2>(startScale, endScale, endScale.X / 10000, Vector2.Lerp);
            tintLerp = new LerpData<Color>(startTint, endTint, (float)endTint.A / 3000f, Color.Lerp);
        }


        public override void Update(GameTime gameTime)
        {
            Scale = scaleLerp.GetCurrent();
            Tint = tintLerp.GetCurrent();
        }


        public void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
        }
    }
}
