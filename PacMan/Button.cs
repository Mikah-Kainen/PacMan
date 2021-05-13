using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Button : Sprite
    {
        InputManager inputManager;
        public Button(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, Vector2 origin, InputManager input)
            : base(tex, tint, pos, scale, origin)
        {
            inputManager = input;
        }

        public bool IsClicked()
        {
            return inputManager.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && HitBox.Contains(inputManager.MouseState.Position);
        }

        public bool IsRightClicked()
        {
            return inputManager.MouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && HitBox.Contains(inputManager.MouseState.Position);
        }
    }
}
