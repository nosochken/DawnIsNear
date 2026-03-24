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
        
        private readonly HashSet<ITargetable> _absorbables = new();
        private readonly HashSet<ITargetable> _absorbers = new();
        private readonly Dictionary<IAbsorbable, ITargetable> _trackedAbsorbables = new();
        private readonly Dictionary<IAbsorber, ITargetable> _trackedAbsorbers = new();

        internal IReadOnlyCollection<ITargetable> Absorbables => _absorbables;
        internal IReadOnlyCollection<ITargetable> Absorbers => _absorbers;
        
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
            
            if (!other.TryGetComponent(out ITargetable targetable))
                return;

            if (other.TryGetComponent(out IAbsorbable absorbable))
            {
                _absorbables.Add(targetable);

                if (_trackedAbsorbables.TryAdd(absorbable, targetable))
                    absorbable.Absorbed += OnAbsorbed;
            }

            if (other.TryGetComponent(out IAbsorber absorber))
            {
                _absorbers.Add(targetable);

                if (_trackedAbsorbers.TryAdd(absorber, targetable))
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
        
        private bool IsOwner(Collider2D other)
        {
            return other.transform == _ownerTransform || other.transform.IsChildOf(_ownerTransform);
        }

        private void RemoveAbsorbable(IAbsorbable absorbable)
        {
            if (_trackedAbsorbables.Remove(absorbable, out ITargetable targetable))
            {
                absorbable.Absorbed -= OnAbsorbed;
                _absorbables.Remove(targetable);
            }
        }

        private void RemoveAbsorber(IAbsorber absorber)
        {
            if (_trackedAbsorbers.Remove(absorber, out ITargetable targetable))
            {
                absorber.BecameInactive -= OnAbsorberBecameInactive;
                _absorbers.Remove(targetable);
            }
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