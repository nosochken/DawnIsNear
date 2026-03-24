using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(AbsorbableDetector))]
    public class Absorber : MonoBehaviour
    {
        private Size _size;
        private AbsorbableDetector _absorbableDetector;
        
        public bool IsActive => isActiveAndEnabled;
        public int CurrentSize => _size.Current;
        public Vector2 CurrentPosition => transform.position;

        internal void Initialize(Size size)
        {
            _size = size ?? throw new ArgumentNullException(nameof(size));
            _absorbableDetector = GetComponent<AbsorbableDetector>();
        }
        
        private void Awake()
        {
            _absorbableDetector = GetComponent<AbsorbableDetector>();
        }

        private void OnEnable()
        {
            _absorbableDetector.Detected +=  OnDetected;
        }

        private void OnDisable()
        {
            _absorbableDetector.Detected -=  OnDetected;
        }
        
        private void Absorb(IAbsorbable absorbable)
        {
            _size.Increase(absorbable.CurrentSize);
            absorbable.BeAbsorbed();
        }
        
        private void OnDetected(IAbsorbable absorbable)
        {
            if (absorbable.Body.Size.Current <= _size.Current)
                Absorb(absorbable);
        }
    }
}