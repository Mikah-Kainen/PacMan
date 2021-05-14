using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.TraversalStuff
{
    public static class Traversals
    {


        public static Stack<T> AStar<T>(T startingPosition, T targetPosition, Func<T /*currentPosition*/, T /*targetPosition*/, int /*tentativeDistance*/> heuristicFunction, T[,] grid, T previousPosition)
            where T : IComparable<T>, ITraversable<T>
        {
            if (startingPosition.PositionInGrid.Equals(targetPosition.PositionInGrid))
            {
                return null;
            }
            Stack<T> returnStack = new Stack<T>();
            Heap<T> priorityQueue = new Heap<T>();
            foreach (T value in grid)
            {
                value.FinalDistance = double.MaxValue;
                value.KnownDistance = double.MaxValue;
                value.WasVisited = false;
                value.Founder = default;
            }

            startingPosition.KnownDistance = 0;
            startingPosition.FinalDistance = heuristicFunction(startingPosition, targetPosition);
            priorityQueue.Add(startingPosition);
            T finalPosition = default;

            while (priorityQueue.Count > 0)
            {
                T currentPosition = priorityQueue.Pop();
                if (currentPosition.PositionInGrid.Equals(targetPosition.PositionInGrid))
                {
                    finalPosition = currentPosition;
                    break;
                }

                foreach (T neightbor in currentPosition.Neighbors)
                {
                    if (!neightbor.IsObstacle && !neightbor.Equals(previousPosition))
                    {
                        if (neightbor.KnownDistance > currentPosition.KnownDistance + currentPosition.Weight)
                        {
                            neightbor.Founder = currentPosition;
                            neightbor.KnownDistance = currentPosition.KnownDistance + currentPosition.Weight;
                            neightbor.FinalDistance = neightbor.KnownDistance + heuristicFunction(neightbor, targetPosition);
                            neightbor.WasVisited = false;
                        }
                        
                        if (!priorityQueue.Contains(neightbor) && !neightbor.WasVisited)
                        {
                            priorityQueue.Add(neightbor);
                        }
                    }
                }
                currentPosition.WasVisited = true;
                previousPosition = currentPosition;
            }
        
            
            while (finalPosition.Founder != null)
            {
                returnStack.Push(finalPosition);
                finalPosition = finalPosition.Founder;
            }

            return returnStack;
        }

    }
}
