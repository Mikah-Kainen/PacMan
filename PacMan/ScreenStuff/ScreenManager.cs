using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class ScreenManager
    {
        public Screen CurrentScreen { get; private set; }
        public Settings Settings { get; private set; }
        public MyStack<Screen> PreviousScreens { get; private set; }

        public Dictionary<Screens, Screen> ScreenDictionary { get; set; }
        public ScreenManager(Settings settings)
        {
            Settings = settings;
            PreviousScreens = new MyStack<Screen>();
            ScreenDictionary = new Dictionary<Screens, Screen>();
        }

        public void Add(Screens name, Screen screen)
        {
            ScreenDictionary.Add(name, screen);
        }

        public void SetScreen(Screens name)
        {
            if (!ScreenDictionary.ContainsKey(name)) { return; }
            Screen currentScreen = ScreenDictionary[name];
            PreviousScreens.Push(CurrentScreen);
            CurrentScreen = currentScreen;

        }

        public void LeaveScreen()
        {
            if (PreviousScreens.Count > 0)
            {
                CurrentScreen = PreviousScreens.Pop();
            }
            else
            {
                CurrentScreen = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Screen screen in PreviousScreens)
            {
                screen.Draw(spriteBatch);
            }
        }

    }
}
