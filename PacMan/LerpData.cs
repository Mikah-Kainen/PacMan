using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    public struct LerpData<T>
    {
        public T Start;
        public T End { get; set; }
        float percentComplete;
        float step;
        Func<T, T, float, T> lerpFunction;

        public bool IsComplete => percentComplete >= 1;

        public LerpData(T start, T end, float step, Func<T, T, float, T> lerpFunction)
        {
            this.Start = start;
            this.End = end;
            this.step = step;
            percentComplete = 0;
            this.lerpFunction = lerpFunction;
        }

        public T GetCurrent()
        {
            if (IsComplete) { return End; }

            T returnValue = lerpFunction(Start, End, percentComplete);
            percentComplete += step;

            return returnValue;
        }

    }
}
