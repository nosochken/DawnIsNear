using System;

namespace Game.Gameplay
{
    internal interface IAbsorber
    {
        public event Action<IAbsorber> BecameInactive;
        
        public EntityType Type { get; }
    }
}