using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class MyStack<T> : IEnumerable
    {
        List<T> supportList;
        public int Count => supportList.Count;
        public MyStack()
        {
            supportList = new List<T>();
        }
        public void Push(T value)
        {
            supportList.Add(value);
        }
        public T Pop()
        {
            T returnValue = supportList[0];
            supportList.Remove(supportList[0]);
            return returnValue;
        }
        public T Peek()
        {
            return supportList[0];
        }

        public IEnumerator GetEnumerator()
        {
            foreach(T value in supportList)
            {
                yield return value;
            }
        }
    }
}
