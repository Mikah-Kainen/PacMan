using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.TraversalStuff
{
    public interface ITraversable<T>
    {
        public List<T> Neighbors { get; set; }
        public bool WasVisited { get; set; }
        public int TentativeDistance { get; set; }
        public Point PositionInGrid { get; set; }

        public bool IsObstacle { get; set; }
    }
}
