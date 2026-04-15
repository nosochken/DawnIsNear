using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    internal class AbsorberOverTime : MonoBehaviour
    {
        private Absorber _absorber;
        private Dictionary<IAbsorbable, Coroutine> _absorbables = new();
        private float _absorbOverTimeInterval;
        
        internal void Initialize(Absorber absorber, float delayInAbsorb)
        {
            _absorber = absorber ?? throw new ArgumentNullException(nameof(absorber));
            
            if (delayInAbsorb <= 0f)
                throw new ArgumentOutOfRangeException(nameof(delayInAbsorb));
            
            _absorbOverTimeInterval = delayInAbsorb;
        }

        internal void StartAbsorbGradually(IAbsorbable absorbable)
        {
            if (_absorbables.ContainsKey(absorbable))
                return;
            
            Coroutine absorbCoroutine = StartCoroutine(AbsorbOverTime(absorbable));
            _absorbables.Add(absorbable, absorbCoroutine);
        }

        internal void StopAbsorbGradually(IAbsorbable absorbable)
        {
            if (_absorbables.TryGetValue(absorbable, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
                _absorbables.Remove(absorbable);
            }
        }
        
        internal void StopAllAbsorb()
        {
            foreach (Coroutine coroutine in _absorbables.Values)
                StopCoroutine(coroutine);

            _absorbables.Clear();
        }
        
        private IEnumerator AbsorbOverTime(IAbsorbable absorbable)
        {
            WaitForSeconds wait = new WaitForSeconds(_absorbOverTimeInterval);
            
            while (absorbable != null && _absorber.CanAbsorb(absorbable))
            {
                _absorber.Absorb(absorbable);
                
                yield return wait;
            }
            
            _absorbables.Remove(absorbable);
        }
    }
}