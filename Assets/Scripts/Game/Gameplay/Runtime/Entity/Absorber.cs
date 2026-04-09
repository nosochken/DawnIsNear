using System;
using UnityEngine;

namespace Game.Gameplay
{
    internal class Absorber : MonoBehaviour, IAbsorber
    {
        private EntityType _type;
        private Size _size;
        
        public event Action<IAbsorber> BecameInactive;
        
        public IBody Body { get; private set; }

        internal void Initialize(EntityType type, IBody body, Size size)
        {
            _type = type;
            Body = body ?? throw new ArgumentNullException(nameof(body));
            _size = size ?? throw new ArgumentNullException(nameof(size));
        }

        private void OnDisable()
        {
            BecameInactive?.Invoke(this);
        }

        public bool CanAbsorb(IAbsorbable absorbable)
        {
            if (AbsorptionRule.CanAbsorb(_type, absorbable.Type)) 
                return absorbable.Body.Size.Current <= _size.Current;
            
            return false;
        }
        
        public void Absorb(IAbsorbable absorbable)
        {
            _size.Increase(absorbable.Body.Size.Current);
            absorbable.BeAbsorbed();
        }
    }
}