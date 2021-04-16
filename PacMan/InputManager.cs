using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class InputManager
    {
        public KeyboardState KeyboardState;
        public MouseState MouseState;
        public void Update()
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
        }
    }
}
