using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
    internal class EnemySpaceScanner: MonoBehaviour
    {
        private CircleCollider2D _collider;
        private Rigidbody2D _rigidbody;
        
        private readonly HashSet<IAbsorbable> _absorbables = new();
        private readonly HashSet<IAbsorber> _absorbers = new();
        private readonly HashSet<IAbsorbable> _tracked = new();

        internal IReadOnlyCollection<IAbsorbable> Absorbables => _absorbables;
        internal IReadOnlyCollection<IAbsorber> Absorbers => _absorbers;
        
        public void Construct(float scannerRadius)
        {
            _collider.radius = scannerRadius;
        }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IAbsorbable absorbable))
                return;
            
            if (_tracked.Add(absorbable))
                absorbable.Absorbed += OnAbsorbed;
            
            if (absorbable is IAbsorber absorber)
                _absorbers.Add(absorber);
            else
                _absorbables.Add(absorbable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IAbsorbable absorbable))
                return;

            Remove(absorbable);
        }

        private void Remove(IAbsorbable absorbable)
        {
            if (absorbable is IAbsorber absorber)
                _absorbers.Remove(absorber);
            else
                _absorbables.Remove(absorbable);
            
            if (_tracked.Remove(absorbable))
                absorbable.Absorbed -= OnAbsorbed;
        }
        
        private void OnAbsorbed(IAbsorbable absorbable)
        {
            Remove(absorbable);
        }
    }
}