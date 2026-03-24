using System;

namespace Game.Gameplay
{
    public interface ISizeEvents
    {
        public event Action<int> SizeChanged;
    }
}