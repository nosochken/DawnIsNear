using System;
using UnityEngine;

namespace Game.Gameplay
{
    internal class Absorber : MonoBehaviour, IAbsorber
    {
        private EntityType _type;
        private IAbsorptionPolicy _absorptionPolicy;
        private Size _size;
        
        public event Action<IAbsorber> BecameInactive;
        
        public IBody Body { get; private set; }

        internal void Initialize(EntityType type, IAbsorptionPolicy absorptionPolicy, IBody body = null, Size size = null)
        {
            _type = type;
            _absorptionPolicy = absorptionPolicy ?? throw new ArgumentNullException(nameof(absorptionPolicy));
            Body = body;
            _size = size;
        }

        private void OnDisable()
        {
            BecameInactive?.Invoke(this);
        }

        public bool CanAbsorb(IAbsorbable absorbable)
        {
            if (AbsorptionRule.CanAbsorb(_type, absorbable.Type))
                return _absorptionPolicy.CanAbsorb(absorbable, _size);
            
            return false;
        }
        
        public void Absorb(IAbsorbable absorbable)
        {
           _absorptionPolicy.Absorb(absorbable, _size);
        }
    }
}