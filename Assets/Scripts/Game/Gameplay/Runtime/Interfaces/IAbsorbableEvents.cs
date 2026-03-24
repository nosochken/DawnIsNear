using System;

namespace Game.Gameplay
{
    public interface IAbsorbableEvents
    {
        public event Action<IAbsorbable> Absorbed;
    }
}