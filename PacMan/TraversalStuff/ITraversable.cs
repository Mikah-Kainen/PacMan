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
        public double FinalDistance { get; set; }
        public double KnownDistance { get; set; }
        public Point PositionInGrid { get; set; }
        public bool IsObstacle { get; }
    }
}
