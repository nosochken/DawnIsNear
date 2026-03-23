using System;

namespace Game.Gameplay
{
    public class Size
    {
        public event Action<int> SizeChanged;
        
        public int Min { get; private set; }
        public int Current { get; private set; }

        internal Size(int min)
        {
            Min = min;
            SetMin();
        }

        internal void SetMin()
        {
            Current = Min;
            SizeChanged?.Invoke(Current);
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
    }
}