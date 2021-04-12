using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.TraversalStuff
{
    public static class Traversals
    {


        public static Stack<T> AStar<T>(ITraversable<T> startingPosition, ITraversable<T> targetPosition, Func<T /*currentPosition*/, T /*targetPosition*/, int /*tentativeDistance*/> heuristicFunction, T[,] grid)
        {
            Stack<T> returnStack = new Stack<T>();






            return returnStack;
        }
    }
}
