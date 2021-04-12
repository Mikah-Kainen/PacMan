using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.TraversalStuff
{
    public static class Traversals
    {


        public static void AStar<T>(ITraversable<T> startingPosition, ITraversable<T> targetPosition, Func<T /*currentPosition*/, T /*targetPosition*/, int /*tentativeDistance*/> heuristicFunction)
        {

        }
    }
}
