using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    internal class AbsorbableDetector : MonoBehaviour
    {
        private CapsuleCollider2D _collider;
        
        internal event Action<IAbsorbable> Detected;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.TryGetComponent(out IAbsorbable depletable))
                Detected?.Invoke(depletable);
        }
    }
}