using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static PacMan.Enum;

namespace PacMan
{
    public class Ghost : AnimationSprite
    {
        private Pacman targetPac;
        private List<Tile> walls;
        private List<Ghost> otherGhosts;
        private float speedPerSecond;
        private int iterationsPerUpdate;
        private ScreenManager screenManager;
        private InputManager inputManager;

        public Ghost(Texture2D tex, Color tint, Vector2 pos, Vector2 scale, List<AnimationFrame> frames, TimeSpan timeBetweenFrames, Pacman targetPac, List<Tile> walls, List<Ghost> otherGhosts, float speedPerUpdate, int iterationsPerUpdate, ScreenManager screenManager, InputManager inputManager)
            :base(tex, tint, pos, scale, frames, timeBetweenFrames)
        {
            this.targetPac = targetPac;
            this.walls = walls;
            this.otherGhosts = otherGhosts;
            this.speedPerSecond = speedPerSecond;
            this.iterationsPerUpdate = iterationsPerUpdate;
            this.screenManager = screenManager;
            this.inputManager = inputManager;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }


        private Directions CalculateDirection()
        {
            return Directions.None;
        }
    }
}
