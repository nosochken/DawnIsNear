using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(AbsorbableDetector))]
    public class Absorber : MonoBehaviour, IAbsorber
    {
        private Size _size;
        private AbsorbableDetector _absorbableDetector;
        
        public event Action<IAbsorber> BecameInactive;
        
        public bool IsActive => isActiveAndEnabled;
        public int CurrentSize => _size.Current;
        public Vector2 CurrentPosition => transform.position;

        internal void Initialize(Size size)
        {
            _size = size ?? throw new ArgumentNullException(nameof(size));
            _absorbableDetector = GetComponent<AbsorbableDetector>();
        }

        private void OnEnable()
        {
            _absorbableDetector.Detected +=  OnDetected;
        }

        private void OnDisable()
        {
            _absorbableDetector.Detected -=  OnDetected;
            
            BecameInactive?.Invoke(this);
        }
        
        private void Absorb(IAbsorbable absorbable)
        {
            _size.Increase(absorbable.Size.Current);
            absorbable.BeAbsorbed();
        }
        
        private void OnDetected(IAbsorbable absorbable)
        {
            if (absorbable.Size.Current <= _size.Current)
                Absorb(absorbable);
        }
    }
}