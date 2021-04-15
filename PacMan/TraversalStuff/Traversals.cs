using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.TraversalStuff
{
    public static class Traversals
    {


        public static Stack<T> AStar<T>(T startingPosition, T targetPosition, Func<T /*currentPosition*/, T /*targetPosition*/, int /*tentativeDistance*/> heuristicFunction, T[,] grid)
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
            startingPosition.WasVisited = true;
            priorityQueue.Add(startingPosition);
            T finalPosition = RecursiveAStar<T>(priorityQueue, targetPosition, heuristicFunction);

            while (!finalPosition.PositionInGrid.Equals(startingPosition.PositionInGrid))
            {
                returnStack.Push(finalPosition);
                finalPosition = finalPosition.Founder;
            }

            return returnStack;
        }

        private static T RecursiveAStar<T>(Heap<T> priorityQueue, T targetPosition, Func<T /*currentPosition*/, T /*targetPosition*/, int /*tentativeDistance*/> heuristicFunction)
            where T : IComparable<T>, ITraversable<T>
        {
            if (priorityQueue.Count == 0)
            {
                throw new Exception("No suitable path was found");
            }
            T currentPosition = priorityQueue.Pop();
            if (currentPosition.PositionInGrid.Equals(targetPosition.PositionInGrid))
            {
                return currentPosition;
            }

            foreach (T neightbor in currentPosition.Neighbors)
            {
                if (!neightbor.IsObstacle && !neightbor.WasVisited)
                {
                    if (neightbor.KnownDistance > currentPosition.KnownDistance + currentPosition.Weight)
                    {
                        neightbor.Founder = currentPosition;
                        neightbor.KnownDistance = currentPosition.KnownDistance + currentPosition.Weight;
                        neightbor.FinalDistance = neightbor.KnownDistance + heuristicFunction(neightbor, targetPosition);
                        priorityQueue.Add(neightbor);
                    }
                    else if(!priorityQueue.Contains(neightbor))
                    {
                        priorityQueue.Add(neightbor);
                    }
                }
            }
            currentPosition.WasVisited = true;
            return RecursiveAStar(priorityQueue, targetPosition, heuristicFunction);
        }
    }
}
