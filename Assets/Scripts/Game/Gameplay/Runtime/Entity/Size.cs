using System;

namespace Game.Gameplay
{
    public class Size : ISizeData, ISizeEvents
    {
        public event Action<int> SizeChanged;
        
        public int Min { get; private set; }
        public int Current { get; private set; }

        internal Size(int min)
        {
            Min = min;
            ResetToMin();
        }

        internal void Increase(int value)
        {
            Current += value;
            SizeChanged?.Invoke(Current);
        }

        internal void Decrease(int value)
        {
            Current -= value;
            SizeChanged?.Invoke(Current);
        }

        internal void DecreaseToZero()
        {
            Current = 0;
            SizeChanged?.Invoke(Current);
        }
        
        private void ResetToMin()
        {
            Current = Min;
            SizeChanged?.Invoke(Current);
        }
    }
}