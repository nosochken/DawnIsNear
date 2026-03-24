using System;

namespace Game.Gameplay
{
    public interface IAbsorber
    {
        public event Action<IAbsorber> BecameInactive;
    }
}