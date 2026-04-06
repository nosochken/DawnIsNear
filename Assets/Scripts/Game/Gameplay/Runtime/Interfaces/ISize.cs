using System;

namespace Game.Gameplay
{
    public interface ISize
    {
        public event Action<int> Changed;
        
        public int Min { get; }
        public int Current { get; }
    }
}