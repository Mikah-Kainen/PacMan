using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacMan.TraversalStuff;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PacMan
{
    public class Tile : Sprite, ITraversable<Tile>, IComparable<Tile>
    {
        public TileTypes TileType;

        private Vector2 pos;
        public override ref Vector2 Pos => ref pos;

        public List<Tile> Neighbors { get; set; }

        public Point PositionInGrid { get; set; }

        public double FinalDistance { get; set; }

        public double KnownDistance { get; set; }

        public bool WasVisited { get; set; }

        public Tile Founder { get; set; }

        public double Weight { get; set; }

        public bool IsObstacle { get; set; }

        public Tile(Texture2D tex, Color tint, Vector2 scale, TileTypes tileType, Point posInGrid)
            : base(tex, tint, Vector2.Zero, scale, /*new Vector2(.5f * scale.X * tex.Width, .5f * scale.Y * tex.Height)*/ Vector2.Zero)
        {
            TileType = tileType;
            Neighbors = new List<Tile>();
            this.PositionInGrid = posInGrid;
            WasVisited = false;
            Weight = 1;

            IsObstacle = tileType == TileTypes.Wall || tileType == TileTypes.Teleport;
            pos = new Vector2(posInGrid.X * Scale.X * tex.Width + Origin.X, posInGrid.Y * Scale.Y * tex.Height + Origin.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public int CompareTo(Tile other)
        {
            return FinalDistance.CompareTo(other.FinalDistance);
        }

        public bool Equals(Tile other)
        {
            return PositionInGrid.Equals(other.PositionInGrid);
        }
        
    }
}
