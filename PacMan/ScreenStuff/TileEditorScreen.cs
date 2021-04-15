using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace PacMan.ScreenStuff
{
    class TileEditorScreen : Screen
    {
        //
        public TileEditorScreen(GraphicsDevice graphicsDevice)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            var result = dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                var texture = Texture2D.FromStream(graphicsDevice, dialog.OpenFile());
            }

        }
    }
}
