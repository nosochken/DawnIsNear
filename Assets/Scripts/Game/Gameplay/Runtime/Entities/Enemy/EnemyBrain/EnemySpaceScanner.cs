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
        private Transform _ownerTransform;
        
        private readonly HashSet<IAbsorbable> _absorbables = new();
        private readonly HashSet<IAbsorber> _absorbers = new();

        internal IReadOnlyCollection<IAbsorbable> Absorbables => _absorbables;
        internal IReadOnlyCollection<IAbsorber> Absorbers => _absorbers;
        
        internal void Initialize(Transform ownerTransform, float scannerRadius)
        {
            _ownerTransform = ownerTransform ?? throw new ArgumentNullException(nameof(ownerTransform));
            
            if (scannerRadius <= 0f)
                throw new ArgumentOutOfRangeException(nameof(scannerRadius));
            
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
            if (IsOwner(other))
                return;

            if (other.TryGetComponent(out IAbsorbable absorbable))
            {
                if (_absorbables.Add(absorbable))
                    absorbable.Absorbed += OnAbsorbed;
            }

            if (other.TryGetComponent(out IAbsorber absorber))
            {
                if (_absorbers.Add(absorber))
                    absorber.BecameInactive += OnAbsorberBecameInactive;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsOwner(other))
                return;
            
            if (other.TryGetComponent(out IAbsorbable absorbable))
                RemoveAbsorbable(absorbable);
            
            if (other.TryGetComponent(out IAbsorber absorber))
                RemoveAbsorber(absorber);
        }
        
        private void OnDisable()
        {
            foreach (IAbsorbable absorbable in _absorbables)
                absorbable.Absorbed -= OnAbsorbed;

            foreach (IAbsorber absorber in _absorbers)
                absorber.BecameInactive -= OnAbsorberBecameInactive;

            _absorbables.Clear();
            _absorbers.Clear();
        }
        
        private bool IsOwner(Collider2D other)
        {
            return other.transform == _ownerTransform || other.transform.IsChildOf(_ownerTransform);
        }

        private void RemoveAbsorbable(IAbsorbable absorbable)
        {
            if (_absorbables.Remove(absorbable))
                absorbable.Absorbed -= OnAbsorbed;
        }

        private void RemoveAbsorber(IAbsorber absorber)
        {
            if (_absorbers.Remove(absorber))
                absorber.BecameInactive -= OnAbsorberBecameInactive;
        }
        
        private void OnAbsorbed(IAbsorbable absorbable)
        {
            RemoveAbsorbable(absorbable);
        }
        
        private void OnAbsorberBecameInactive(IAbsorber absorber)
        {
            RemoveAbsorber(absorber);
        }
    }
}