using System;

namespace Game.Gameplay
{
    public interface IAbsorber
    {
        public event Action<IAbsorber> BecameInactive;
        
        public IBody Body { get; }

        public bool CanAbsorb(IAbsorbable absorbable);
    }
}