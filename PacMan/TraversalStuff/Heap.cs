using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class Heap<T>
        where T : IComparable<T>
    {
        public T Head => backingList[0];
        private List<T> backingList;
        public int Count => backingList.Count;
        public Heap()
        {
            backingList = new List<T>();
        }

        public T Pop()
        {
            T temp = Head;
            backingList[0] = backingList[Count - 1];
            backingList.RemoveAt(Count - 1);

            HeapifyDown(0);

            return temp;
            
        }

        public void Add(T value)
        {
            backingList.Add(value);
            HeapifyUp(Count - 1);
        }

        private void HeapifyUp(int currentIndex)
        {
            if(currentIndex == 0)
            {
                return;
            }
            int parentIndex = GetParent(currentIndex);

            if(backingList[currentIndex].CompareTo(backingList[parentIndex]) < 0)
            {
                Swap(currentIndex, parentIndex);
                HeapifyUp(parentIndex);
            }
        }

        private void HeapifyDown(int currentIndex)
        {
            int leftChild = GetLeftChild(currentIndex);
            int rightChild = GetRightChild(currentIndex);

            int compIndex;
            if (leftChild >= Count)
            {
                return;
            }
            else if (rightChild >= Count)
            {
                compIndex = leftChild;
            }
            else
            {
                if (backingList[leftChild].CompareTo(backingList[rightChild]) < 0)
                {
                    compIndex = leftChild;
                }
                else
                {
                    compIndex = rightChild;
                }
            }

            if(backingList[compIndex].CompareTo(backingList[currentIndex]) < 0)
            {
                Swap(compIndex, currentIndex);
                HeapifyDown(compIndex);
            }
        }

        public bool Contains(T value)
        {
            foreach(T indexValue in backingList)
            {
                if(indexValue.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        private void Swap(int index1, int index2)
        {
            T temp = backingList[index1];
            backingList[index1] = backingList[index2];
            backingList[index2] = temp;
        }
        private int GetParent(int currentIndex) => (currentIndex - 1) / 2;
        private int GetLeftChild(int currentIndex) => currentIndex * 2 + 1;
        private int GetRightChild(int currentIndex) => currentIndex * 2 + 2;
    }
}
