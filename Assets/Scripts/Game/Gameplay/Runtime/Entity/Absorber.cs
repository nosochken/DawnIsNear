using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(AbsorbableDetector))]
    public class Absorber : MonoBehaviour
    {
        private Unit _unit;
        private AbsorbableDetector _absorbableDetector;

        internal void Initialize(Unit unit)
        {
            _unit = unit ?? throw new ArgumentNullException(nameof(unit));
            
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
            _unit.Body.Size.Increase(absorbable.Body.Size.Current);
            absorbable.BeAbsorbed();
        }
        
        private void OnDetected(IAbsorbable absorbable)
        {
            if (absorbable.Body.Size.Current <= _unit.Body.Size.Current)
                Absorb(absorbable);
        }
    }
}