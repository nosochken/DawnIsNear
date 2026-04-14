using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    internal class AbsorbableDetector : MonoBehaviour
    {
        private CapsuleCollider2D _collider;
        
        internal event Action<IAbsorbable> Detected;
        internal event Action<IAbsorbable> Lost;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.TryGetComponent(out IAbsorbable absorbable))
                Detected?.Invoke(absorbable);
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.TryGetComponent(out IAbsorbable absorbable))
                Lost?.Invoke(absorbable);
        }
    }
}