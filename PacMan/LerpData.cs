using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public class LerpData<T>
    {
        T start;
        public T End { get; set; }
        float percentComplete;
        float step;
        Func<T, T, float, T> lerpFunction;

        public bool IsCompleted => percentComplete >= 1;

        public LerpData(T start, T end, float step, Func<T, T, float, T> lerpFunction)
        {
            this.start = start;
            this.End = end;
            this.step = step;
            percentComplete = 0;
            this.lerpFunction = lerpFunction;
        }

        public T GetCurrent()
        {
            if (IsCompleted) { return End; }

            T returnValue = lerpFunction(start, End, percentComplete);
            percentComplete += step;

            return returnValue;
        }

    }
}
