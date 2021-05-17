using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PacMan.ScreenStuff;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace PacMan
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public Screen CurrentScreen;
        private ScreenManager screenManager;
        private InputManager inputManager;
        private Settings settings;
        private Vector2 screenSize;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            screenSize = new Vector2(800, 800);
            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            settings = new Settings(GraphicsDevice);
            screenManager = new ScreenManager(settings);
            inputManager = new InputManager();
            //screenManager.Add(Screens.Editor, new TileEditorScreen(graphics, Content, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), screenManager, inputManager));
            screenManager.Add(Screens.Game, new GameScreen(graphics, Content, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), screenManager, inputManager));



            screenManager.SetScreen(Screens.Game);
            screenManager.CurrentScreen.Init();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            inputManager.Update();
            screenManager.CurrentScreen.Update(gameTime);

            ////////////////////////////////////////////////
            // TODO: Add your update logic here
            /////////////////////////////////////////

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            screenManager.CurrentScreen.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
