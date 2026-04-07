using System;
using UnityEngine;

namespace Game.Gameplay
{
    internal class Absorber : MonoBehaviour, IAbsorber
    {
        private Size _size;
        
        public event Action<IAbsorber> BecameInactive;
        
        public EntityType Type { get; private set; }

        internal void Initialize(EntityType type, Size size)
        {
            Type = type;
            _size = size ?? throw new ArgumentNullException(nameof(size));
        }

        private void OnDisable()
        {
            BecameInactive?.Invoke(this);
        }

        public bool CanAbsorb(IAbsorbable absorbable)
        {
            return absorbable.Size.Current <= _size.Current;
        }
        
        public void Absorb(IAbsorbable absorbable)
        {
            _size.Increase(absorbable.Size.Current);
            absorbable.BeAbsorbed();
        }
    }
}