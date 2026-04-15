using System;

namespace Game.Gameplay
{
    public class Size : ISize
    {
        public event Action<int> Changed;
        
        public int Min { get; private set; }
        public int Current { get; private set; }

        internal Size(int min)
        {
            Min = min;
            ResetToMin();
        }

        internal void SetCurrent(int value)
        {
            Current = value;
            Changed?.Invoke(Current);
        }

        internal void Increase(int value)
        {
            Current += value;
            Changed?.Invoke(Current);
        }

        internal void Decrease(int value)
        {
            Current -= value;
            Changed?.Invoke(Current);
        }

        internal void DecreaseToZero()
        {
            Current = 0;
            Changed?.Invoke(Current);
        }
        
        private void ResetToMin()
        {
            Current = Min;
            Changed?.Invoke(Current);
        }
    }
}