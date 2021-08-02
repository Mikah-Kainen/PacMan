using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.TraversalStuff
{
    public static class Traversals<T> where T : IComparable<T>, ITraversable<T>
    {
    

        public static Stack<T> AStar(T startingPosition, T targetPosition, Func<T /*currentPosition*/, T /*targetPosition*/, int /*tentativeDistance*/> heuristicFunction, T[,] grid, T previousPosition)
        { 
            targetPosition = FindClosestTarget(startingPosition, targetPosition, heuristicFunction, grid, previousPosition);

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
                        if (neightbor.KnownDistance > currentPosition.KnownDistance + neightbor.Weight)
                        {
                            neightbor.Founder = currentPosition;
                            neightbor.KnownDistance = currentPosition.KnownDistance + neightbor.Weight;
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
                //previousPosition = currentPosition;
            }

            //if final is null return no path
            if (finalPosition != null)
            {
                while (finalPosition.Founder != null)
                {
                    returnStack.Push(finalPosition);
                    finalPosition = finalPosition.Founder;
                }
            }

            return returnStack;
        }


        public static T FindClosestTarget(T startingPosition, T targetPosition, Func<T /*currentPosition*/, T /*targetPosition*/, int /*tentativeDistance*/> heuristicFunction, T[,] grid, T previous)
        {
            if(!startingPosition.Equals(targetPosition) && !targetPosition.IsObstacle)
            {
                return targetPosition;
            }

            T closest = default(T);
            //Queue, enqueue the start,
            //Loop: dequeue, enqueue the dequed node's neighbors
            //If the node's neighbors are empty return that dequed neighbors node

            HashSet<T> PossibleTargets = new HashSet<T>();
            Queue<T> backingQueue = new Queue<T>();
            backingQueue.Enqueue(targetPosition);

            while (backingQueue.Count > 0)
            {
                T current = backingQueue.Dequeue();
                if (IsInBounds(new Point(current.PositionInGrid.X + 1, current.PositionInGrid.Y), grid))
                {
                    T temp = grid[current.PositionInGrid.Y, current.PositionInGrid.X + 1];
                    if (!temp.IsObstacle)
                    {
                        PossibleTargets.Add(temp);
                    }
                    else
                    {
                        backingQueue.Enqueue(temp);
                    }
                }
                if (IsInBounds(new Point(current.PositionInGrid.X - 1, current.PositionInGrid.Y), grid))
                {
                    T temp = grid[current.PositionInGrid.Y, current.PositionInGrid.X - 1];
                    if (!temp.IsObstacle)
                    {
                        PossibleTargets.Add(temp);
                    }
                    else
                    {
                        backingQueue.Enqueue(temp);
                    }
                }
                if (IsInBounds(new Point(current.PositionInGrid.X, current.PositionInGrid.Y + 1), grid))
                {
                    T temp = grid[current.PositionInGrid.Y + 1, current.PositionInGrid.X];
                    if (!temp.IsObstacle)
                    {
                        PossibleTargets.Add(temp);
                    }
                    else
                    {
                        backingQueue.Enqueue(temp);
                    }
                }
                if (IsInBounds(new Point(current.PositionInGrid.X, current.PositionInGrid.Y - 1), grid))
                {
                    T temp = grid[current.PositionInGrid.Y - 1, current.PositionInGrid.X];
                    if (!temp.IsObstacle)
                    {
                        PossibleTargets.Add(temp);
                    }
                    else
                    {
                        backingQueue.Enqueue(temp);
                    }
                }

                if(PossibleTargets.Contains(startingPosition))
                {
                    PossibleTargets.Remove(startingPosition); 
                }
                if(PossibleTargets.Contains(previous))
                {
                    PossibleTargets.Remove(previous);
                }
                if (PossibleTargets.Count > 0)
                {
                    int leastDistance = int.MaxValue;
                    foreach (T target in PossibleTargets)
                    {
                        int temp = heuristicFunction(startingPosition, target);
                        if (temp < leastDistance)
                        {
                            leastDistance = temp;
                            closest = target;
                        }
                    }
                    return closest;
                }
            }

            return previous;
        }

        public static bool IsInBounds(Point currentPoint, T[,] grid)
        {
            return currentPoint.X < grid.GetLength(1) && currentPoint.X >= 0  && currentPoint.Y < grid.GetLength(0) && currentPoint.Y >= 0;
        }
    }
}
