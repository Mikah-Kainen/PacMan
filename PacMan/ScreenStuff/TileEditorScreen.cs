using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace PacMan.ScreenStuff
{
    class TileEditorScreen : Screen
    {
        GraphicsDevice graphicsDevice => GraphicsDeviceManager.GraphicsDevice;
        Texture2D pixelMap;
        //
        public TileEditorScreen(GraphicsDeviceManager graphics, ContentManager content, Rectangle bounds, ScreenManager screenManager, InputManager inputManager)
        {
            base.Load(graphics, content, bounds, screenManager, inputManager);

            OpenFileDialog dialog = new OpenFileDialog();

            var result = dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                pixelMap = Texture2D.FromStream(graphicsDevice, dialog.OpenFile());
            }



        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixelMap, Bounds, Color.White);
        }
    }
}
